using Unity.Mathematics;

namespace Flexy.Tweens;

public interface ITweenAdapter<T> where T: unmanaged		
{ 
	public T Lerp( T a, T b, Single t ); 
}

public struct		Adapter_Single		: ITweenAdapter<Single>		{ public Single		Lerp( Single a, Single b, Single t )		=> Mathf.LerpUnclamped( a, b, t ); }
public struct		Adapter_Vector2		: ITweenAdapter<Vector2>	{ public Vector2	Lerp( Vector2 a, Vector2 b, Single t )		=> Vector2.LerpUnclamped( a, b, t ); }
public struct		Adapter_Vector3		: ITweenAdapter<Vector3>	{ public Vector3	Lerp( Vector3 a, Vector3 b, Single t )		=> Vector3.LerpUnclamped( a, b, t ); }
public struct		Adapter_Vector4		: ITweenAdapter<Vector4>	{ public Vector4	Lerp( Vector4 a, Vector4 b, Single t )		=> Vector4.LerpUnclamped( a, b, t ); }
public struct		Adapter_Color		: ITweenAdapter<Color>		{ public Color		Lerp( Color a, Color b, Single t )			=> Color.LerpUnclamped( a, b, t ); }
public struct		Adapter_Rect		: ITweenAdapter<Rect>		{ public Rect		Lerp( Rect a, Rect b, Single t )			=> new( math.lerp( a.min, b.min, t ), math.lerp( a.size, b.size, t ) ); }
										
public struct		Adapter_Int32		: ITweenAdapter<Int32>		{ public Int32		Lerp( Int32 a, Int32 b, Single t )			=> (Int32)Mathf.LerpUnclamped( a, b, t ); }
										
public struct		Adapter_Int64		: ITweenAdapter<Int64>		{ public Int64		Lerp( Int64 a, Int64 b, Single t )			=> (Int64)math.lerp( (Double)a, b, t ); }
public struct		Adapter_Double		: ITweenAdapter<Double>		{ public Double		Lerp( Double a, Double b, Single t )		=> math.lerp( a, b, t ); }
public struct		Adapter_Color32		: ITweenAdapter<Color32>	{ public Color32	Lerp( Color32 a, Color32 b, Single t )		=> Color32.LerpUnclamped( a, b, t );  }
public struct		Adapter_Quaternion	: ITweenAdapter<Quaternion>	{ public Quaternion	Lerp( Quaternion a, Quaternion b, Single t )=> Quaternion.LerpUnclamped( a, b, t ); }