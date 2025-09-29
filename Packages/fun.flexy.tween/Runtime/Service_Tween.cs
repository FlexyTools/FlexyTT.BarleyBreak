using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Flexy.Tweens
{
	[BurstCompile]
	[DefaultExecutionOrder(Int32.MinValue+100)]
	public class Service_Tween : MonoBehaviour
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void Init() 
		{
			Global			= null;
			_serviceById	= new (256);
			_nextServiceId	= 1;
			_safetyHandle	= AtomicSafetyHandle.Create(); 
		}
		
		[SerializeField] ETimeSource _timeSource;
		
		public static Service_Tween Global { get; private set; }
		private static Dictionary<UInt16, Service_Tween> _serviceById;
		private static AtomicSafetyHandle _safetyHandle;
		private static UInt16 _nextServiceId = 1;
		
		private UInt16 _id;
		private readonly Dictionary<UInt16, TweenRunner> _runnersById = new(256);
		private readonly Dictionary<Type, TweenRunner> _runnersByType = new(256);
		private UInt16 _nextRunnerId = 1;
		
		internal static TweenRunner? GetRunner	( UInt16 serviceId, UInt16 runnerId )	
		{
			if (!_serviceById.TryGetValue(serviceId, out var service))
				return null;
			
			return service._runnersById[runnerId];
		}
		
		public	TweenHandle		Run<T, TLerp>	( TweenParams<T, TLerp> tween ) where T : unmanaged where TLerp : unmanaged, ITweenAdapter<T>	
		{
			if (!_runnersByType.TryGetValue( typeof(TweenParams<T, TLerp>), out var runner ))
			{
				runner = new TweenRunner<T, TLerp>();
				runner.Id = _nextRunnerId++;
				runner.ServiceId = _id;
				_runnersByType.Add( typeof(TweenParams<T, TLerp>), runner );
				_runnersById.Add( runner.Id, runner );
			}
			
			return ((TweenRunner<T, TLerp>)runner).Run( tween );
		}

		public	void			Advance ( Single deltaTime )
		{
			foreach (var r in _runnersById)
				r.Value.Advance( deltaTime );
		}
		
		private	void			Awake	( )		
		{
			_id = _nextServiceId++;
			_serviceById.Add(_id, this);
			
			if (Global == null)
				Global = this;
		}
		private	void			Update	( )		
		{
			switch (_timeSource)
			{
				case ETimeSource.DeltaTime:			Advance( Time.deltaTime );			break;
				case ETimeSource.UnscaledDeltaTime:	Advance( Time.unscaledDeltaTime );	break;
			}
		}
		
		internal abstract class TweenRunner
		{
			public UInt16 ServiceId { get; set; }
			public UInt16 Id { get; set; }
			
			public abstract		void			Complete	( UInt32 tweenId );
			public abstract		void			Cancel		( UInt32 tweenId );
			public abstract		Boolean			IsValid		( UInt32 tweenId );
			public abstract		UniTask.Awaiter	Await		( UInt32 tweenId );
			
			public abstract		void	Advance	( Single deltaTime );
		}
		
		private class TweenRunner<T, TAdapter> : TweenRunner where T : unmanaged where TAdapter : unmanaged, ITweenAdapter<T>
		{
			private readonly	Dictionary<UInt32, Int32> _idToIndex = new(64);
			private				TweenHighData[]		_runningHigh	= new TweenHighData[64];
			private 			TweenSegmentSoa64	_runningLow;
			
			private ProfilerMarker _marker_update		= new ( $"Tweens Run<{typeof(T).Name}, {typeof(TAdapter).Name}>" );
			private ProfilerMarker _marker_advance		= new ( "Advance" );
			private ProfilerMarker _marker_adapt		= new ( "Adapt" );
			private ProfilerMarker _marker_callback		= new ( "Callbacks" );
			
			private readonly	Dictionary<Int32, TweenHighData>						_scheduledTweens	= new (256);
			private readonly	Dictionary<Int32, (Int32 nextId, ScaledSegment seg)>	_nextSegments		= new (256);
			private				Int32			_count;
			private				UInt32			_nextTweenId = 1;
			private				Int32			_nextSegmentId = 1;
			private				Int32			_nextScheduledId = 1;

			public				TweenHandle		Run			( TweenParams<T, TAdapter> tweenParams )	
			{
				Debug.LogError( $"[Tween] Run duration: {tweenParams.Duration}" );
				
				var tween = new TweenHighData 
				{
					Id = _nextTweenId++,
				
					Bindings = tweenParams.Bindings,
					Callbacks = tweenParams.Callbacks,

					Duration = tweenParams.Duration,
					Adapter = tweenParams.Adapter,

					From = tweenParams.From,
					To = tweenParams.To,
					Current = tweenParams.From,
					 
					Segment = tweenParams.Segments[0].segment,
					SegmentScale = 1/ tweenParams.Duration, 
				};
				
				while (_idToIndex.ContainsKey( tween.Id ))
					tween.Id++;
				
				tween.SegmentScale /= tweenParams.Segments[0].scale;
				
				if (tweenParams.Segments.Length > 1)
				{
					var nextId = ++_nextSegmentId;
					tween.NextId = nextId;
					
					for (var i = 1; i < tweenParams.Segments.Length; i++)
					{
						var ts = (0, tweenParams.Segments[i]);
						
						if (i + 1 < tweenParams.Segments.Length)
							ts.Item1 = ++_nextSegmentId;
							
						Debug.LogError( $"[Tween] Add segment: {nextId}" );
						_nextSegments.Add(nextId,ts);
						nextId = _nextSegmentId;
					}
				}
				
				if (tweenParams.StartDelay > 0)
				{
					//add to scheduled collection and create delay tween
					var nextId = --_nextScheduledId; 
					_scheduledTweens.Add( nextId, tween );
					Debug.LogError( $"[Tween] Scheduled id: {nextId}" );
					
					tween = new TweenHighData 
					{
						Duration = tweenParams.StartDelay, 
						NextId = nextId, 
						From = default,
						To = default,
						Current = default,
					 
						Segment = EaseUtility.LinearPoly,
						SegmentScale = 1 / tweenParams.StartDelay, 
					};
				}
				
				var index = _count++;
				
				if (_runningLow.Capacity != _runningHigh.Length)
					Array.Resize(ref _runningHigh, _runningLow.Capacity);
				
				_runningHigh[index] = tween; 
				_runningLow.S[index] = tween.SegmentScale;	
				_runningLow.T[index] = tween.CurrT;		
				_runningLow.Y[index] = tween.CurrY;		
				_runningLow.A[index] = tween.Segment._a;		
				_runningLow.B[index] = tween.Segment._b;		
				_runningLow.C[index] = tween.Segment._c;		
				_runningLow.D[index] = tween.Segment._d;
						
				Debug.LogError( $"[Tween] Running duration: {tween.Duration}" );
				_idToIndex[_nextTweenId] = index;
				return new(){ ServiceId = ServiceId, RunnerId = Id, TweenId = tween.Id };
			}
			public override		void			Advance		( Single deltaTime )	
			{
				using var _ = _marker_update.Auto();
			
				using (_marker_advance.Auto())
					TweenSegmentSoa64.Tweens_Advance	(ref _runningLow, deltaTime, ref _safetyHandle);
					
				using (_marker_adapt.Auto())
					Tweens_Adapt	(_runningLow, _runningHigh);
				
				using var __ = _marker_callback.Auto();
				
				// Invoke callbacks
				for ( var i = 0; i < _runningHigh.Length; i++ )
				{
					var tween = _runningHigh[i];
					if ( tween.Duration == 0 ) //Empty
						continue;
					
					tween.CurrT = _runningLow.T[i];		
					tween.CurrY = _runningLow.Y[i];	
					
					if( tween.CurrT >= 1 )
					{
						Debug.LogError( $"[Tween] t > 1" );
						
						if (tween.NextId > 0)
						{
							while (tween.NextId > 0)
							{
								var newT = tween.CurrT - 1.0f; 
								
								Debug.LogError( $"[Tween] Remove segment: {tween.NextId}" );
								_nextSegments.Remove(tween.NextId, out var next);
								
								var d1 = 1/tween.Duration;
								newT *= d1 / tween.SegmentScale / next.seg.scale;
								
								Debug.LogError( $"[Tween] next segment t: {newT} oldT: {tween.CurrT-1}" );
								
								tween.NextId = next.nextId;
								tween.Segment = next.seg.segment;
								tween.CurrT = newT;
								tween.SegmentScale = 1 / tween.Duration;
								tween.SegmentScale /= next.seg.scale;
								tween.Advance(0); 
								
								if (newT > 1)
									continue;
								
								_runningHigh[i] = tween;
								_runningLow.S[i] = tween.SegmentScale;	
								_runningLow.T[i] = tween.CurrT;		
								_runningLow.Y[i] = tween.CurrY;		
								_runningLow.A[i] = tween.Segment._a;		
								_runningLow.B[i] = tween.Segment._b;		
								_runningLow.C[i] = tween.Segment._c;		
								_runningLow.D[i] = tween.Segment._d;	
								break;
							}
						}
						else if (tween.NextId < 0)
						{
							while (tween.NextId < 0)
							{
								var newT = tween.CurrT - 1.0f; 
								
								Debug.LogError( $"[Tween] Remove scheduled tween: {tween.NextId}" );
								_scheduledTweens.Remove(tween.NextId, out var next);
								
								newT /= tween.SegmentScale;
								
								Debug.LogError( $"[Tween] next segment t: {newT} oldScale: {tween.SegmentScale}" );
								
								tween = next;
								tween.CurrT = newT * tween.SegmentScale;
								
								if (newT > 1)
									continue;
								
								tween.Advance(0);
								_runningHigh[i] = tween;
								_runningLow.S[i] = tween.SegmentScale;	
								_runningLow.T[i] = tween.CurrT;		
								_runningLow.Y[i] = tween.CurrY;		
								_runningLow.A[i] = tween.Segment._a;		
								_runningLow.B[i] = tween.Segment._b;		
								_runningLow.C[i] = tween.Segment._c;		
								_runningLow.D[i] = tween.Segment._d;	
								Debug.LogError( $"[Tween] Running duration: {tween.Duration}" );
								break;
							}
						}
						else
						{
							Debug.LogError( $"[Tween] Completed: segments: {_nextSegments.Count}" );
						
							tween.CurrT = 1;
							tween.Advance(0);
							var easedT			= tween.CurrY;  
							tween.Current		= tween.Adapter.Lerp( tween.From, tween.To, easedT );
						
							try						{ tween.Callbacks.Completed?.Invoke( ); }
							catch ( Exception ex )	{ Debug.LogException( ex ); }
							
							tween = default;
							_runningHigh[i] = tween;
							_runningLow.S[i] = tween.SegmentScale;	
							_runningLow.T[i] = tween.CurrT;		
							_runningLow.Y[i] = tween.CurrY;		
							_runningLow.A[i] = tween.Segment._a;		
							_runningLow.B[i] = tween.Segment._b;		
							_runningLow.C[i] = tween.Segment._c;		
							_runningLow.D[i] = tween.Segment._d;	
						}
					}
					
					try						{ tween.Bindings.InvokeUnsafe( tween.Current ); }
					catch ( Exception ex )	{ Debug.LogException( ex ); }
					//
					// try
					// {
					// 	managedData.UpdateUnsafe(outputPtr[i]);
					// }
					// catch (Exception ex)
					// {
					// 	MotionDispatcher.GetUnhandledExceptionHandler()?.Invoke(ex);
					// 	if (managedData.CancelOnError)
					// 	{
					// 		state.Status = MotionStatus.Canceled;
					// 		managedData.OnCancelAction?.Invoke();
					// 	}
					// }
					// if (state.WasLoopCompleted)
					// {
					// 	managedData.InvokeOnLoopComplete(state.CompletedLoops);
					// }
					//
					// if (status is MotionStatus.Completed && state.WasStatusChanged)
					// {
					// 	managedData.InvokeOnComplete();
					// }
				}
			}
			
			public override		void			Complete	( UInt32 tweenId )		
			{
				if (!_idToIndex.TryGetValue(tweenId, out var index))
					return;
			
				if( _runningHigh.Length <= index )
					return;
				
				var tween = _runningHigh[index];
				
				try						{ tween.Bindings.InvokeUnsafe( tween.To ); }
				catch ( Exception ex )	{ Debug.LogException( ex ); }
				
				try						{ tween.Callbacks.Completed( ); }
				catch ( Exception ex )	{ Debug.LogException( ex ); }
				
				RemoveTweenAtSwapBack(tweenId, index);
			}
			public override		void			Cancel		( UInt32 tweenId )		
			{
				if (!_idToIndex.TryGetValue(tweenId, out var index))
					return;
			
				if( _runningHigh.Length <= index )
					return;
				
				var tween = _runningHigh[index];
				
				try						{ tween.Callbacks.Canceled( ); }
				catch ( Exception ex )	{ Debug.LogException( ex ); }
				
				RemoveTweenAtSwapBack(tweenId, index);
			}
			public override		Boolean			IsValid		( UInt32 tweenId )		
			{
				if (!_idToIndex.TryGetValue(tweenId, out var index))
					return false;
			
				if( _runningHigh.Length <= index )
					return false;
				
				return true;
			}
			public override		UniTask.Awaiter	Await		( UInt32 tweenId )		
			{
				if( !IsValid( tweenId ) )
					return UniTask.CompletedTask.GetAwaiter();
				
				return Awaiter( _idToIndex, tweenId ).GetAwaiter();
				
				static async UniTask Awaiter( Dictionary<UInt32, Int32> idToIndex, UInt32 id )
				{
					while( idToIndex.ContainsKey(id) )
						await UniTask.NextFrame();
				}
			}
			
			private static		void			Tweens_Adapt	( TweenSegmentSoa64 low, TweenHighData[] high )	
			{
				var count = low.Capacity;
				for ( var i = 0; i < count; i++ )
				{
					var easedT		= low.Y[i];  
					var tween		= high[i];
					tween.Current	= tween.Adapter.Lerp( tween.From, tween.To, easedT );
					high[i]			= tween;
				}
			}
			private static		void			Tweens_GoNext	( TweenSegmentSoa64 low )						
			{
			
			}
			private void RemoveTweenAtSwapBack(UInt32 tweenId, Int32 index)
			{
				_idToIndex[_runningHigh[_count-1].Id] = index;
				_runningHigh[index] = _runningHigh[--_count];
				_idToIndex.Remove(tweenId);
			}
			
			private struct TweenHighData
			{
				public	T			From;
				public	T			To;
				public	T			Current;
				public	TAdapter	Adapter;
			
				public	TweenSegment Segment;
				public	Single		SegmentScale;
				public	Single		CurrT;
				public	Single		CurrY;

				public	UInt32		Id;				
				public	Int32		NextId;
				public	Single		Duration;
				
				public	TweenBindings	Bindings;
				public	TweenCallbacks	Callbacks;
				

				public void Advance	( Single deltaTime )
				{
					// t += delta * Scale;
					CurrT = math.mad(deltaTime, SegmentScale, CurrT);
					CurrY = Segment.Evaluate(CurrT);
				}
			}
		}
	}
}