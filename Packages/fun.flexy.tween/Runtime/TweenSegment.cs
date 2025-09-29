using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;

namespace Flexy.Tweens;

[BurstCompile]
[Serializable] 
public struct TweenSegment
{
	public TweenSegment(Keyframe k0, Keyframe k1):this(k0.value, k0.outTangent, k1.inTangent, k1.value)
	{
		
	}
	
	public TweenSegment(Single v0, Single t0, Single t1, Single v1)
	{
		var duration = 1;
	
		this.v0 = v0;
		this.t0 = t0;
		this.t1 = t1;
		this.v1 = v1;
	
		_d = v0;
		_c = duration * t0;
		_b = 3 * (v1 - v0) - 2 * duration * t0 - duration * t1;
		_a = 2 * (v0 - v1) + duration * t0 + duration * t1;
	}

	public	Single v0, t0, t1, v1;
	
	[Space(20)]
	public	Single	_a;
	public	Single	_b;
	public	Single	_c;
	public	Single	_d;
	
	[BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true, OptimizeFor = OptimizeFor.Performance)]
	public static 	Single	Evaluate	( in TweenSegment seg, Single t ) => seg.Evaluate( t );
	
	public float Evaluate(Single t)
	{
		// a*t^3 + b*t^2 + c*t + d
		// var p = ((_a * t + _b) * t + _c) * t + _d;
		var p = math.mad(math.mad(math.mad(_a, t, _b), t, _c), t, _d);
		return p;
	}
	
	public void Log()
	{
		Debug.Log( $"({v0:F4}f, {t0:F4}f, {t1:F4}f, {v1:F4}f)" );
	}

	public Span<ScaledSegment> GetSegmentsSpan()
	{
		var na = new NativeArray<ScaledSegment>(1, Allocator.Temp);
		na[0] = new (1, this);
		return na.AsSpan();
	}
}

public record struct ScaledSegment(Single scale, TweenSegment segment);

[BurstCompile]
[Serializable] 
public struct DiSegment
{
	public DiSegment( Keyframe k0, Keyframe k1, Keyframe k2 ) : this(
		k0.value, k0.outTangent * 0.50f, k1.inTangent * 0.50f,
		k1.value, k1.outTangent * 0.50f, k2.inTangent * 0.50f, k2.value
	) { }

	public DiSegment( Single v0, Single t01, Single t02, Single v1, Single t11, Single t12, Single v2 )
	{
		_0 = new TweenSegment(v0, t01, t02, v1);
		_1 = new TweenSegment(v1, t11, t12, v2);
	}

	public TweenSegment _0;
	public TweenSegment _1;
	
	public Single Evaluate(Single bigT)
	{
		bigT *= 2;
		bigT -= 0.00001f;
		var index = (Int32)bigT;
		
		var seg = index switch
		{
			0 => _0,
			1 => _1,
		};
		
		var t = bigT % 1;
		return seg.Evaluate(t);
	}
	
	public void Log()
	{
		Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f)" );
	}

	public Span<ScaledSegment> GetSegmentsSpan()
	{
		var na = new NativeArray<ScaledSegment>(2, Allocator.Temp);
		na[0] = new(0.5f,_0);
		na[1] = new(0.5f,_1);
		
		return na.AsSpan();
	}
}

[BurstCompile]
[Serializable] 
public struct TetraSegment
{
	public TetraSegment( Keyframe k0, Keyframe k1, Keyframe k2, Keyframe k3, Keyframe k4 ) : this(
		k0.value, k0.outTangent * 0.25f, k1.inTangent * 0.25f,
		k1.value, k1.outTangent * 0.25f, k2.inTangent * 0.25f,
		k2.value, k2.outTangent * 0.25f, k3.inTangent * 0.25f,
		k3.value, k3.outTangent * 0.25f, k4.inTangent * 0.25f, k4.value 
	) { }

	public TetraSegment( Single v0, Single t01, Single t02, Single v1, Single t11, Single t12, Single v2, Single t21, Single t22, Single v3, Single t31, Single t32, Single v4, Single t1 = 0, Single t2 = 0, Single t3 = 0 )
	{
		_0 = new TweenSegment(v0, t01, t02, v1);
		_1 = new TweenSegment(v1, t11, t12, v2);
		_2 = new TweenSegment(v2, t21, t22, v3);
		_3 = new TweenSegment(v3, t31, t32, v4);
		
		_t1 = t1;
		_t2 = t2;
		_t3 = t3;
	}

	public TweenSegment _0;
	public Single _t1;
	public TweenSegment _1;
	public Single _t2;
	public TweenSegment _2;
	public Single _t3;
	public TweenSegment _3;

	public Single Evaluate(Single bigT)
	{
		if (_t1==0)
		{
			bigT *= 4;
			bigT -= 0.00001f;
			var index = (Int32)bigT;
			
			var seg = index switch
			{
				0 => _0,
				1 => _1,
				2 => _2,
				3 => _3, 
			};
			
			var t = bigT % 1;
			return seg.Evaluate(t);
		}
		
		{
			if (bigT < _t1)		return _0.Evaluate(bigT/_t1);
			if (bigT < _t2)		return _1.Evaluate((bigT-_t1)/(_t2-_t1));
			if (bigT < _t3)		return _2.Evaluate((bigT-_t2)/(_t3-_t2));
			
			return _3.Evaluate((bigT-_t3)/(1-_t3));
		}
	}
	
	public void Log()
	{
		if (_t1==0)
			Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f, {_2.t0:F4}f,{_2.t1:F4}f, {_2.v1:F4}f, {_3.t0:F4}f,{_3.t1:F4}f, {_3.v1:F4}f)" );
		else
			Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f, {_2.t0:F4}f,{_2.t1:F4}f, {_2.v1:F4}f, {_3.t0:F4}f,{_3.t1:F4}f, {_3.v1:F4}f, {_t1:F3}f, {_t2:F3}f, {_t3:F3}f)" );
	}

	public Span<ScaledSegment> GetSegmentsSpan()
	{
		var na = new NativeArray<ScaledSegment>(4, Allocator.Temp);
		
		if (_t1 == 0)
		{
			na[0] = new(0.25f,_0);
			na[1] = new(0.25f,_1);
			na[2] = new(0.25f,_2);
			na[3] = new(0.25f,_3);
		}
		else
		{
			na[0] = new(_t1 - 0,	_0);
			na[1] = new(_t2 - _t1,	_1);
			na[2] = new(_t3 - _t2,	_2);
			na[3] = new(1 - _t3,	_3);
		}
		
		return na.AsSpan();
	}
}

[BurstCompile]
[Serializable] 
public struct PentaSegment
{
	public PentaSegment( Keyframe k0, Keyframe k1, Keyframe k2, Keyframe k3, Keyframe k4, Keyframe k5 ) : this(
		k0.value, k0.outTangent * 0.20f, k1.inTangent * 0.20f,
		k1.value, k1.outTangent * 0.20f, k2.inTangent * 0.20f,
		k2.value, k2.outTangent * 0.20f, k3.inTangent * 0.20f,
		k3.value, k3.outTangent * 0.20f, k4.inTangent * 0.20f, 
		k4.value, k4.outTangent * 0.20f, k5.inTangent * 0.20f, k5.value 
	) { }

	public PentaSegment( Single v0, Single t01, Single t02, Single v1, Single t11, Single t12, Single v2, Single t21, Single t22, Single v3, Single t31, Single t32, Single v4, Single t41, Single t42, Single v5, Single t1 = 0, Single t2 = 0, Single t3 = 0, Single t4 = 0 )
	{
		_0 = new TweenSegment(v0, t01, t02, v1);
		_1 = new TweenSegment(v1, t11, t12, v2);
		_2 = new TweenSegment(v2, t21, t22, v3);
		_3 = new TweenSegment(v3, t31, t32, v4);
		_4 = new TweenSegment(v4, t41, t42, v5);
		
		_t1 = t1;
		_t2 = t2;
		_t3 = t3;
		_t4 = t4;
	}

	public TweenSegment _0;
	public Single _t1;
	public TweenSegment _1;
	public Single _t2;
	public TweenSegment _2;
	public Single _t3;
	public TweenSegment _3;
	public Single _t4;
	public TweenSegment _4;

	public Single Evaluate(Single bigT)
	{
		if (_t1==0)
		{
			bigT *= 5;
			bigT -= 0.00001f;
			var index = (Int32)bigT;
			
			var seg = index switch
			{
				0 => _0,
				1 => _1,
				2 => _2,
				3 => _3,
				4 => _4,
			};
			
			var t = bigT % 1;
			return seg.Evaluate(t);
		}
		
		{
			if (bigT < _t1)		return _0.Evaluate(bigT/_t1);
			if (bigT < _t2)		return _1.Evaluate((bigT-_t1)/(_t2-_t1));
			if (bigT < _t3)		return _2.Evaluate((bigT-_t2)/(_t3-_t2));
			if (bigT < _t4)		return _3.Evaluate((bigT-_t3)/(_t4-_t3));
			
			return _4.Evaluate((bigT-_t4)/(1-_t4));
		}
	}
	
	public void Log()
	{
		if (_t1 == 0)
			Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f, {_2.t0:F4}f,{_2.t1:F4}f, {_2.v1:F4}f, {_3.t0:F4}f,{_3.t1:F4}f, {_3.v1:F4}f, {_4.t0:F4}f,{_4.t1:F4}f, {_4.v1:F4}f)" );
		else
			Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f, {_2.t0:F4}f,{_2.t1:F4}f, {_2.v1:F4}f, {_3.t0:F4}f,{_3.t1:F4}f, {_3.v1:F4}f, {_4.t0:F4}f,{_4.t1:F4}f, {_4.v1:F4}f, {_t1:F3}f, {_t2:F3}f, {_t3:F3}f, {_t4:F3}f)" );
	}

	public Span<ScaledSegment> GetSegmentsSpan()
	{
		var na = new NativeArray<ScaledSegment>(5, Allocator.Temp);
		
		if (_t1 == 0)
		{
			na[0] = new(0.2f, _0);
			na[1] = new(0.2f, _1);
			na[2] = new(0.2f, _2);
			na[3] = new(0.2f, _3);
			na[4] = new(0.2f, _4);
		}
		else
		{
			na[0] = new(_t1 - 0,	_0);
			na[1] = new(_t2 - _t1,	_1);
			na[2] = new(_t3 - _t2,	_2);
			na[3] = new(_t4 - _t3,	_3);
			na[4] = new(1 - _t4,	_4);
		}
		
		return na.AsSpan();
	}
}

[BurstCompile]
[Serializable] 
public struct HexaSegment
{
	public HexaSegment( Keyframe k0, Keyframe k1, Keyframe k2, Keyframe k3, Keyframe k4, Keyframe k5, Keyframe k6 ) : this(
		k0.value, k0.outTangent * 0.125f, k1.inTangent * 0.125f,
		k1.value, k1.outTangent * 0.125f, k2.inTangent * 0.125f,
		k2.value, k2.outTangent * 0.125f, k3.inTangent * 0.125f,
		k3.value, k3.outTangent * 0.125f, k4.inTangent * 0.125f, 
		k4.value, k4.outTangent * 0.125f, k5.inTangent * 0.125f,
		k5.value, k5.outTangent * 0.125f, k6.inTangent * 0.125f, k6.value
	) { }

	public HexaSegment( Single v0, Single t01, Single t02, Single v1, Single t11, Single t12, Single v2, Single t21, Single t22, Single v3, Single t31, Single t32, Single v4, Single t41, Single t42, Single v5, Single t51, Single t52, Single v6, Single t1 = 0, Single t2 = 0, Single t3 = 0, Single t4 = 0, Single t5 = 0 )
	{
		_0 = new TweenSegment(v0, t01, t02, v1);
		_1 = new TweenSegment(v1, t11, t12, v2);
		_2 = new TweenSegment(v2, t21, t22, v3);
		_3 = new TweenSegment(v3, t31, t32, v4);
		_4 = new TweenSegment(v4, t41, t42, v5);
		_5 = new TweenSegment(v5, t51, t52, v6);
		
		_t1 = t1;
		_t2 = t2;
		_t3 = t3;
		_t4 = t4;
		_t5 = t5;
	}

	public TweenSegment _0;
	public Single _t1;
	public TweenSegment _1;
	public Single _t2;
	public TweenSegment _2;
	public Single _t3;
	public TweenSegment _3;
	public Single _t4;
	public TweenSegment _4;
	public Single _t5;
	public TweenSegment _5;

	public Single Evaluate(Single bigT)
	{
		if (_t1==0)
		{
			bigT *= 6;
			bigT -= 0.00001f;
			var index = (Int32)bigT;
			
			var seg = index switch
			{
				0 => _0,
				1 => _1,
				2 => _2,
				3 => _3,
				4 => _4,
				5 => _5,
			};
			
			var t = bigT % 1;
			return seg.Evaluate(t);
		}
		
		{
			if (bigT < _t1)		return _0.Evaluate(bigT/_t1);
			if (bigT < _t2)		return _1.Evaluate((bigT-_t1)/(_t2-_t1));
			if (bigT < _t3)		return _2.Evaluate((bigT-_t2)/(_t3-_t2));
			if (bigT < _t4)		return _3.Evaluate((bigT-_t3)/(_t4-_t3));
			if (bigT < _t5)		return _4.Evaluate((bigT-_t4)/(_t5-_t4));
			
			return _5.Evaluate((bigT-_t5)/(1-_t5));
		}
	}
	
	public void Log()
	{
		if (_t1 == 0)
			Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f, {_2.t0:F4}f,{_2.t1:F4}f, {_2.v1:F4}f, {_3.t0:F4}f,{_3.t1:F4}f, {_3.v1:F4}f, {_4.t0:F4}f,{_4.t1:F4}f, {_4.v1:F4}f, {_5.t0:F4}f,{_5.t1:F4}f, {_5.v1:F4}f)" );
		else
			Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f, {_2.t0:F4}f,{_2.t1:F4}f, {_2.v1:F4}f, {_3.t0:F4}f,{_3.t1:F4}f, {_3.v1:F4}f, {_4.t0:F4}f,{_4.t1:F4}f, {_4.v1:F4}f, {_5.t0:F4}f,{_5.t1:F4}f, {_5.v1:F4}f, {_t1:F3}f, {_t2:F3}f, {_t3:F3}f, {_t4:F3}f, {_t5:F3}f)" );
	}

	public Span<ScaledSegment> GetSegmentsSpan()
	{
		var na = new NativeArray<ScaledSegment>(6, Allocator.Temp);
		
		if (_t1 == 0)
		{
			na[0] = new(0.166667f, _0);
			na[1] = new(0.166667f, _1);
			na[2] = new(0.166667f, _2);
			na[3] = new(0.166667f, _3);
			na[4] = new(0.166667f, _4);
			na[5] = new(0.166667f, _5);
		}
		else
		{
			na[0] = new(_t1 - 0,	_0);
			na[1] = new(_t2 - _t1,	_1);
			na[2] = new(_t3 - _t2,	_2);
			na[3] = new(_t4 - _t3,	_3);
			na[4] = new(_t5 - _t4,	_4);
			na[5] = new(1 - _t5,	_5);
		}
		
		return na.AsSpan();
	}
}

[BurstCompile]
[Serializable] 
public struct OctaSegment
{
	public OctaSegment( Keyframe k0, Keyframe k1, Keyframe k2, Keyframe k3, Keyframe k4, Keyframe k5, Keyframe k6, Keyframe k7, Keyframe k8 ) : this(
		k0.value, k0.outTangent * 0.125f, k1.inTangent * 0.125f,
		k1.value, k1.outTangent * 0.125f, k2.inTangent * 0.125f,
		k2.value, k2.outTangent * 0.125f, k3.inTangent * 0.125f,
		k3.value, k3.outTangent * 0.125f, k4.inTangent * 0.125f, 
		k4.value, k4.outTangent * 0.125f, k5.inTangent * 0.125f,
		k5.value, k5.outTangent * 0.125f, k6.inTangent * 0.125f,
		k6.value, k6.outTangent * 0.125f, k7.inTangent * 0.125f,
		k7.value, k7.outTangent * 0.125f, k8.inTangent * 0.125f, k8.value 
	) { }

	public OctaSegment( Single v0, Single t01, Single t02, Single v1, Single t11, Single t12, Single v2, Single t21, Single t22, Single v3, Single t31, Single t32, Single v4, Single t41, Single t42, Single v5, Single t51, Single t52, Single v6, Single t61, Single t62, Single v7, Single t71, Single t72, Single v8, Single t1 = 0, Single t2 = 0, Single t3 = 0, Single t4 = 0, Single t5 = 0, Single t6 = 0, Single t7 = 0 )
	{
		_0 = new TweenSegment(v0, t01, t02, v1);
		_1 = new TweenSegment(v1, t11, t12, v2);
		_2 = new TweenSegment(v2, t21, t22, v3);
		_3 = new TweenSegment(v3, t31, t32, v4);
		_4 = new TweenSegment(v4, t41, t42, v5);
		_5 = new TweenSegment(v5, t51, t52, v6);
		_6 = new TweenSegment(v6, t61, t62, v7);
		_7 = new TweenSegment(v7, t71, t72, v8);
		
		_t1 = t1;
		_t2 = t2;
		_t3 = t3;
		_t4 = t4;
		_t5 = t5;
		_t6 = t6;
		_t7 = t7; 
	}

	public TweenSegment _0;
	public Single _t1;
	public TweenSegment _1;
	public Single _t2;
	public TweenSegment _2;
	public Single _t3;
	public TweenSegment _3;
	public Single _t4;
	public TweenSegment _4;
	public Single _t5;
	public TweenSegment _5;
	public Single _t6;
	public TweenSegment _6;
	public Single _t7;
	public TweenSegment _7;

	public Single Evaluate(Single bigT)
	{
		if (_t1==0)
		{
			bigT *= 8;
			bigT -= 0.00001f;
			var index = (Int32)bigT;
			
			var seg = index switch
			{
				0 => _0,
				1 => _1,
				2 => _2,
				3 => _3,
				4 => _4,
				5 => _5,
				6 => _6,
				7 => _7,
			};
			
			var t = bigT % 1;
			return seg.Evaluate(t);
		}
		
		{
			if (bigT < _t1)		return _0.Evaluate(bigT/_t1);
			if (bigT < _t2)		return _1.Evaluate((bigT-_t1)/(_t2-_t1));
			if (bigT < _t3)		return _2.Evaluate((bigT-_t2)/(_t3-_t2));
			if (bigT < _t4)		return _3.Evaluate((bigT-_t3)/(_t4-_t3));
			if (bigT < _t5)		return _4.Evaluate((bigT-_t4)/(_t5-_t4));
			if (bigT < _t6)		return _5.Evaluate((bigT-_t5)/(_t6-_t5));
			if (bigT < _t7)		return _6.Evaluate((bigT-_t6)/(_t7-_t6));
			
			return _7.Evaluate((bigT-_t7)/(1-_t7));
		}
	}
	
	public void Log()
	{
		if (_t1 == 0)
			Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f, {_2.t0:F4}f,{_2.t1:F4}f, {_2.v1:F4}f, {_3.t0:F4}f,{_3.t1:F4}f, {_3.v1:F4}f, {_4.t0:F4}f,{_4.t1:F4}f, {_4.v1:F4}f)" );
		else
			Debug.Log( $"({_0.v0:F4}f, {_0.t0:F4}f,{_0.t1:F4}f, {_0.v1:F4}f, {_1.t0:F4}f,{_1.t1:F4}f, {_1.v1:F4}f, {_2.t0:F4}f,{_2.t1:F4}f, {_2.v1:F4}f, {_3.t0:F4}f,{_3.t1:F4}f, {_3.v1:F4}f, {_4.t0:F4}f,{_4.t1:F4}f, {_4.v1:F4}f, {_5.t0:F4}f,{_5.t1:F4}f, {_5.v1:F4}f, {_6.t0:F4}f,{_6.t1:F4}f, {_6.v1:F4}f, {_7.t0:F4}f,{_7.t1:F4}f, {_7.v1:F4}f, {_t1:F3}f, {_t2:F3}f, {_t3:F3}f, {_t4:F3}f, {_t5:F3}f, {_t6:F3}f, {_t7:F3}f)" );
	}

	public Span<ScaledSegment> GetSegmentsSpan()
	{
		var na = new NativeArray<ScaledSegment>(8, Allocator.Temp);
		
		if (_t1 == 0)
		{
			na[0] = new(0.125f, _0);
			na[1] = new(0.125f, _1);
			na[2] = new(0.125f, _2);
			na[3] = new(0.125f, _3);
			na[4] = new(0.125f, _4);
			na[5] = new(0.125f, _5);
			na[6] = new(0.125f, _6);
			na[7] = new(0.125f, _7);
		}
		else
		{
			na[0] = new(_t1 - 0,	_0);
			na[1] = new(_t2 - _t1,	_1);
			na[2] = new(_t3 - _t2,	_2);
			na[3] = new(_t4 - _t3,	_3);
			na[4] = new(_t5 - _t4,	_4);
			na[5] = new(_t6 - _t5,	_5);
			na[6] = new(_t7 - _t6,	_6);
			na[7] = new(1 - _t7,	_7);
		}
		
		return na.AsSpan();
	}
}