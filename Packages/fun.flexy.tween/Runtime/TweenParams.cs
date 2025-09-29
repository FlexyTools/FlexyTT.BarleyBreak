using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace Flexy.Tweens;

public ref struct TweenParams<T, TAdapter> where T : unmanaged where TAdapter : unmanaged, ITweenAdapter<T>
{
	public	TweenParams	( Service_Tween svc, T from, T to, Single duration, Ease ease = Ease.InOutSine, Single startDelay = 0, TAdapter adapter = default )
	{
		Svc	= svc;
	
		From		= from;
		To			= to;
		Adapter		= adapter;
		
		StartDelay	= startDelay;
		Duration	= duration;
		TimeSourceOverride	= default;
		
		Bindings	= default;
		Callbacks	= default;
		
		Segments	= EaseUtility.GetSegments(ease);
	}
	
	internal	Service_Tween	Svc;
	
	internal	T				From;
	internal	T				To;
	internal	TAdapter		Adapter;
	
	internal	Single			StartDelay;
	internal	Single			Duration;
	internal	ETimeSource		TimeSourceOverride;
	
	internal	TweenBindings	Bindings;
	internal	TweenCallbacks	Callbacks;
	internal	Span<ScaledSegment> Segments;
	
	public	TweenParams<T, TAdapter> WithDelay				( Single startDelay )		{ StartDelay = startDelay;	return this; }
	public	TweenParams<T, TAdapter> OverrideTimeSource		( ETimeSource src )			{ TimeSourceOverride = src;	return this; }
	
	public	TweenParams<T, TAdapter> OnComplete				( Action callback )			{ Callbacks.Completed	= callback;		return this; }
	public	TweenParams<T, TAdapter> OnCancel				( Action callback )			{ Callbacks.Canceled	= callback;		return this; }
	
	public	TweenParams<T, TAdapter> BindTo					(							Action<T>				evaluator )	{ Bindings = TweenBindings.Create( evaluator );				return this; }
	public	TweenParams<T, TAdapter> BindTo<TO1>			( TO1 o1,					Action<T, TO1>			evaluator )	{ Bindings = TweenBindings.Create( evaluator, o1 );			return this; }
	public	TweenParams<T, TAdapter> BindTo<TO1, TO2>		( TO1 o1, TO2 o2,			Action<T, TO1,TO2>		evaluator )	{ Bindings = TweenBindings.Create( evaluator, o1, o2 );		return this; }
	public	TweenParams<T, TAdapter> BindTo<TO1, TO2, TO3>	( TO1 o1, TO2 o2, TO3 o3,	Action<T, TO1,TO2,TO3>	evaluator )	{ Bindings = TweenBindings.Create( evaluator, o1, o2, o3 );	return this; }
	
	public 	TweenHandle Run( ) => Svc.Run( this );
}

public enum		ETimeSource : byte
{
	None				= 0,
	DeltaTime			= 1,
	UnscaledDeltaTime	= 2,
}

public struct TweenHandle
{
	public UInt16	ServiceId;
	public UInt16	RunnerId;
	public UInt32	TweenId;
	
	public void				Complete	( ) => Service_Tween.GetRunner(ServiceId, RunnerId)?.Complete	( TweenId );
	public void				Cancel		( ) => Service_Tween.GetRunner(ServiceId, RunnerId)?.Cancel		( TweenId );
	public Boolean			IsValid		( ) => Service_Tween.GetRunner(ServiceId, RunnerId)?.IsValid	( TweenId ) is true;
	public UniTask.Awaiter	GetAwaiter	( ) => Service_Tween.GetRunner(ServiceId, RunnerId)?.Await		( TweenId ) ?? UniTask.CompletedTask.GetAwaiter();
}

public struct TweenCallbacks
{
	public	Action	Completed;
	public	Action	Canceled;
}

public struct TweenBindings
{
	public	Object	State1;
	public	Object	State2;
	public	Object	State3;
	private	Object	_boundAction;
	
	public	Object	BoundAction => _boundAction;
	
	public static TweenBindings Create					( Object updateAction )							=> new(){ _boundAction = updateAction };
	public static TweenBindings Create<TO1>				( Object updateAction, TO1 o1 )					=> new(){ _boundAction = updateAction, State1 = o1 };
	public static TweenBindings Create<TO1, TO2>		( Object updateAction, TO1 o1, TO2 o2 )			=> new(){ _boundAction = updateAction, State1 = o1, State2 = o2  };
	public static TweenBindings Create<TO1, TO2, TO3>	( Object updateAction, TO1 o1, TO2 o2, TO3 o3 )	=> new(){ _boundAction = updateAction, State1 = o1, State2 = o2, State3 = o3 };
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void InvokeUnsafe<TValue>(in TValue value) where TValue : unmanaged
	{
		var stateCount = State3 != null ? 3 : State2 != null ? 2 : State1 != null ? 1 : 0;
	
		switch ( stateCount )
		{
			case 0: UnsafeUtility.As<Object, Action<TValue>>						( ref _boundAction )?.Invoke( value );							break;
			case 1: UnsafeUtility.As<Object, Action<TValue, Object>>				( ref _boundAction )?.Invoke( value, State1 );					break;
			case 2: UnsafeUtility.As<Object, Action<TValue, Object, Object>>		( ref _boundAction )?.Invoke( value, State1, State2 );			break;
			case 3: UnsafeUtility.As<Object, Action<TValue ,Object, Object, Object>>( ref _boundAction )?.Invoke( value, State1, State2, State3 );	break;
		}
	}
}