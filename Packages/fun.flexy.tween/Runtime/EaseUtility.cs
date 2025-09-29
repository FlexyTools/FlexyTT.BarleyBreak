using Unity.Burst;
using Unity.Mathematics;

namespace Flexy.Tweens;

[BurstCompile]
public static class EaseUtility
{
	public static readonly Ease Default = Ease.InOutSine;
	
	[BurstCompile]
	public static			Single Evaluate( this Ease ease, Single t )
	{
		return ease switch
		{
			Ease.Linear			=> Linear		(t),
			                	
			Ease.InSine			=> InSine		(t),
			Ease.OutSine		=> OutSine		(t),
			Ease.InOutSine		=> InOutSine	(t),
			                	
			Ease.InQuad			=> InQuad		(t),
			Ease.OutQuad		=> OutQuad		(t),
			Ease.InOutQuad		=> InOutQuad	(t),
			                	
			Ease.InCubic		=> InCubic		(t),
			Ease.OutCubic		=> OutCubic		(t),
			Ease.InOutCubic		=> InOutCubic	(t),
			                	
			Ease.InQuart		=> InQuart		(t),
			Ease.OutQuart		=> OutQuart		(t),
			Ease.InOutQuart		=> InOutQuart	(t),
			                	
			Ease.InQuint		=> InQuint		(t),
			Ease.OutQuint		=> OutQuint		(t),
			Ease.InOutQuint		=> InOutQuint	(t),
			                	
			Ease.InExpo			=> InExpo		(t),
			Ease.OutExpo		=> OutExpo		(t),
			Ease.InOutExpo		=> InOutExpo	(t),
			                	
			Ease.InCirc			=> InCirc		(t),
			Ease.OutCirc		=> OutCirc		(t),
			Ease.InOutCirc		=> InOutCirc	(t),
			                	
			Ease.InBack			=> InBack		(t), 		
			Ease.OutBack		=> OutBack		(t), 		
			Ease.InOutBack		=> InOutBack	(t), 
			
			Ease.InElastic		=> InElastic	(t), 		
			Ease.OutElastic		=> OutElastic	(t), 		
			Ease.InOutElastic	=> InOutElastic	(t), 
			
			Ease.InBounce		=> InBounce		(t), 		
			Ease.OutBounce		=> OutBounce	(t), 		
			Ease.InOutBounce	=> InOutBounce	(t),
			
			_ => Evaluate( Default, t )
		};
	}
	
	public static			Span<ScaledSegment> GetSegments(Ease ease)
	{
		return ease switch
		{
			Ease.Linear			=> LinearPoly		.GetSegmentsSpan(),
			                	
			Ease.InSine			=> InSinePoly		.GetSegmentsSpan(),
			Ease.OutSine		=> OutSinePoly		.GetSegmentsSpan(),	
			Ease.InOutSine		=> InOutSinePoly	.GetSegmentsSpan(),			                	
			Ease.InQuad			=> InQuadPoly		.GetSegmentsSpan(),	
			Ease.OutQuad		=> OutQuadPoly		.GetSegmentsSpan(),	
			Ease.InOutQuad		=> InOutQuadPoly	.GetSegmentsSpan(),			                	
			Ease.InCubic		=> InCubicPoly		.GetSegmentsSpan(),	
			Ease.OutCubic		=> OutCubicPoly		.GetSegmentsSpan(),	
			Ease.InOutCubic		=> InOutCubicPoly	.GetSegmentsSpan(),			                	
			Ease.InQuart		=> InQuartPoly		.GetSegmentsSpan(),	
			Ease.OutQuart		=> OutQuartPoly		.GetSegmentsSpan(),	
			Ease.InOutQuart		=> InOutQuartPoly	.GetSegmentsSpan(),			                	
			Ease.InQuint		=> InQuintPoly		.GetSegmentsSpan(),	
			Ease.OutQuint		=> OutQuintPoly		.GetSegmentsSpan(),	
			Ease.InOutQuint		=> InOutQuintPoly	.GetSegmentsSpan(),			                	
			Ease.InExpo			=> InExpoPoly		.GetSegmentsSpan(),	
			Ease.OutExpo		=> OutExpoPoly		.GetSegmentsSpan(),	
			Ease.InOutExpo		=> InOutExpoPoly	.GetSegmentsSpan(),			                	
			Ease.InCirc			=> InCircPoly		.GetSegmentsSpan(),	
			Ease.OutCirc		=> OutCircPoly		.GetSegmentsSpan(),	
			Ease.InOutCirc		=> InOutCircPoly	.GetSegmentsSpan(),			                	
			Ease.InBack			=> InBackPoly		.GetSegmentsSpan(),	 		
			Ease.OutBack		=> OutBackPoly		.GetSegmentsSpan(),	 		
			Ease.InOutBack		=> InOutBackPoly	.GetSegmentsSpan(),				
			Ease.InElastic		=> InElasticPoly	.GetSegmentsSpan(),	
			Ease.OutElastic		=> OutElasticPoly	.GetSegmentsSpan(), 		
			Ease.InOutElastic	=> InOutElasticPoly	.GetSegmentsSpan(),			
			Ease.InBounce		=> InBouncePoly		.GetSegmentsSpan(),	 		
			Ease.OutBounce		=> OutBouncePoly	.GetSegmentsSpan(),	
			Ease.InOutBounce	=> InOutBouncePoly	.GetSegmentsSpan(),
		};
	}
	
	[BurstCompile] public static 	Single     Linear		( Single t )    => LinearPoly.		Evaluate(t);
	
	[BurstCompile] public static 	Single     InSine		( Single t )    => InSinePoly.		Evaluate(t);
	[BurstCompile] public static 	Single     OutSine		( Single t )    => OutSinePoly.		Evaluate(t);
	[BurstCompile] public static 	Single     InOutSine	( Single t )    => InOutSinePoly.	Evaluate(t);
	
	[BurstCompile] public static 	Single     InQuad		( Single t )    => InQuadPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     OutQuad		( Single t )    => OutQuadPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     InOutQuad	( Single t )    => InOutQuadPoly.	Evaluate(t);
	
	[BurstCompile] public static 	Single     InCubic		( Single t )    => InCubicPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     OutCubic		( Single t )    => OutCubicPoly.	Evaluate(t);
	[BurstCompile] public static 	Single     InOutCubic	( Single t )    => InOutCubicPoly.	Evaluate(t);
	
	[BurstCompile] public static 	Single     InQuart		( Single t )    => InQuartPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     OutQuart		( Single t )    => OutQuartPoly.	Evaluate(t);
	[BurstCompile] public static 	Single     InOutQuart	( Single t )    => InOutQuartPoly.	Evaluate(t);
	
	[BurstCompile] public static 	Single     InQuint		( Single t )    => InQuintPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     OutQuint		( Single t )    => OutQuintPoly.	Evaluate(t);
	[BurstCompile] public static 	Single     InOutQuint	( Single t )    => InOutQuintPoly.	Evaluate(t);
	
	[BurstCompile] public static 	Single     InExpo		( Single t )    => InExpoPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     OutExpo		( Single t )    => OutExpoPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     InOutExpo	( Single t )    => InOutExpoPoly.	Evaluate(t);
	
	[BurstCompile] public static 	Single     InCirc		( Single t )    => InCircPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     OutCirc		( Single t )    => OutCircPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     InOutCirc	( Single t )	=> InOutCircPoly.	Evaluate(t);
	
	[BurstCompile] public static 	Single     InBack		( Single t )    => InBackPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     OutBack		( Single t )    => OutBackPoly.		Evaluate(t);
	[BurstCompile] public static 	Single     InOutBack	( Single t )	=> InOutBackPoly.	Evaluate(t);

	[BurstCompile] public static 	Single     InElastic	( Single t )    => InElasticPoly.	Evaluate(t);
	[BurstCompile] public static 	Single     OutElastic	( Single t )    => OutElasticPoly.	Evaluate(t);
	[BurstCompile] public static 	Single     InOutElastic	( Single t )	=> InOutElasticPoly.Evaluate(t);

	[BurstCompile] public static 	Single     InBounce		( Single t )    => InBouncePoly.	Evaluate(t);
	[BurstCompile] public static 	Single     OutBounce	( Single t )    => OutBouncePoly.	Evaluate(t);
	[BurstCompile] public static 	Single     InOutBounce	( Single t )	=> InOutBouncePoly.	Evaluate(t);

	[BurstCompile] public static	Single     Sqrt			( Single t )	=> SqrtPoly.		Evaluate(t);
	
	
	[BurstCompile] public static	TetraSegment	EarpToTetraSegment ( Single easeIn, Single easeOut )
	{
		TetraSegment segment = default; 
		var delta = 0.0f;
		
		for (var i = 0; i < 4; i++)
		{
			var t0o = Earp( 0.01f, easeIn, easeOut ) * 25;
					
			var p1	= Earp( 0.25f, easeIn, easeOut );
			var t1i	= (p1 - Earp( 0.24f, easeIn, easeOut )) * 25;
			var t1o = (p1 - Earp( 0.26f, easeIn, easeOut )) * -25;
					
			var p2 = Earp( 0.50f, easeIn, easeOut );
			var t2i = (p2 - Earp( 0.499f, easeIn, easeOut )) * 250 * (1+delta);
			var t2o = (p2 - Earp( 0.501f, easeIn, easeOut )) * -250 * (1+delta);
					
			var p3 = Earp( 0.75f, easeIn, easeOut );
			var t3i = (p3 - Earp( 0.74f, easeIn, easeOut )) * 25;
			var t3o = (p3 - Earp( 0.76f, easeIn, easeOut )) * -25;
					
			var t4i = (1f - Earp( 0.99f, easeIn, easeOut )) * 25;
					
			segment	= new TetraSegment	(0, t0o, t1i, p1, t1o, t2i, p2, t2o, t3i, p3, t3o, t4i, 1);
					
			delta -= segment.Evaluate(0.625f) - Earp( 0.625f, easeIn, easeOut ) + (-segment.Evaluate(0.375f) + Earp( 0.375f, easeIn, easeOut ));
					
			if (math.abs(delta) < 0.02f)
				break;
		}
				
		return segment;
	}
	
	[BurstCompile] public static	Single			Earp ( Single t, Single easeIn, Single easeOut, Single start = 0, Single end = 1 )	
	{
		easeIn = Mathf.Clamp(easeIn, 0.001f, 0.999f);
		easeOut = Mathf.Clamp(easeOut, 0.001f, 0.999f);
		var e = easeOut + easeIn;

		var z = easeOut / (easeIn + easeOut);
		z = Mathf.Clamp(z, 0.001f, 0.999f);

		var y = (-((1f - z) / (z * Mathf.Pow(0.5f + t, e * (7.5f + (5f * t))) - z + 1f)) + ((1f - z) / (z * Mathf.Pow(1.5f, e * (12.5f)) - z + 1f))) / (((1f - z) / (z * Mathf.Pow(1.5f, e * (12.5f)) - z + 1f)) - ((1f - z) / (z * Mathf.Pow(0.5f, e * (7.5f)) - z + 1f)));

		return end + ((start - end) * y);
	}
	[BurstCompile] public static	Single			Berp ( Single start, Single end, Single time, Single frequency = 2.7f )		
	{
		// A custom interpolation that oscillates around the end value to create a bounce effect. A larger 'frequency' value increases both the number and magnitude of the oscillations.
		// https://www.desmos.com/calculator/jd06nf9u8s
	
		frequency = Mathf.Clamp(frequency, 0.0f, 50.0f);
		var b = frequency;

		var a = 2.2f - (0.01156f * (frequency - 2.7f));
		var c = 1.2f + (0.01156f * (frequency - 2.7f));

		time = Mathf.Clamp01(time);
		time = (time + Mathf.Pow(1.0f - time, a) * Mathf.Sin(Mathf.PI * b * time * time * time * time)) * (1.0f + c - (c * time));

		return start + (end - start) * time;
	}
	
	// https://www.youtube.com/watch?v=bFOAipGJGA0
	// https://www.ryanjuckett.com/damped-springs/
	
	public static			TweenSegment 	LinearPoly		=> new(0f, 1f, 1f, 1f);
	
	public static  			TweenSegment 	InSinePoly		=> new(0f, 0f, 1.63f, 1f);
	public static  			TweenSegment 	OutSinePoly		=> new(0f, 1.63f, 0f, 1f);
	public static			TweenSegment 	InOutSinePoly	=> new(0f, 0f, 0f, 1f);
	
	public static 			TetraSegment	InQuadPoly		=> new(0.0000f, 0.0000f,0.1270f, 0.0630f, 0.1270f,0.2510f, 0.2500f, 0.2510f,0.3830f, 0.5630f, 0.3830f,0.5080f, 1.0000f);
	public static 			TetraSegment	OutQuadPoly		=> new(0.0000f, 0.4950f,0.3730f, 0.4380f, 0.3730f,0.2510f, 0.7500f, 0.2510f,0.1290f, 0.9370f, 0.1290f,0.0000f, 1.0000f);
	public static 			TetraSegment	InOutQuadPoly	=> new(0.0000f, 0.0000f,0.2480f, 0.1250f, 0.2480f,0.5000f, 0.5000f, 0.5000f,0.2480f, 0.8750f, 0.2480f,0.0000f, 1.0000f);
	
	public static 			TetraSegment 	InCubicPoly		=> new(0.0000f, 0.0000f,0.0440f, 0.0155f, 0.0440f,0.1900f, 0.1250f, 0.1900f,0.4200f, 0.4220f, 0.4200f,0.7500f, 1.0000f);
	public static 			TetraSegment 	OutCubicPoly	=> new(0.0000f, 0.7500f,0.4200f, 0.5780f, 0.4200f,0.1880f, 0.8750f, 0.1880f,0.0450f, 0.9845f, 0.0450f,0.0000f, 1.0000f);
	public static 			TetraSegment 	InOutCubicPoly	=> new(0.0000f, 0.0000f,0.1840f, 0.0620f, 0.1840f,0.7420f, 0.5000f, 0.7420f,0.1840f, 0.9380f, 0.1840f,0.0000f, 1.0000f);
																	
	public static 			DiSegment		InQuartPoly		=> new(0.0000f, 0.0000f,0.2000f, 0.0626f, 0.2000f,1.9626f, 1.0000f);
	public static 			DiSegment		OutQuartPoly	=> new(0.0000f, 2.0000f,0.2000f, 0.9369f, 0.2000f,0.0000f, 1.0000f);
	public static 			TetraSegment	InOutQuartPoly	=> new(0.0000f, 0.0000f,0.0950f, 0.0300f, 0.0950f,0.9460f, 0.5000f, 0.9460f,0.0950f, 0.9700f, 0.0950f,0.0000f, 1.0000f);
																		
	public static 			DiSegment		InQuintPoly		=> new(0.0000f, 0.0000f,0.0881f, 0.0300f, 0.0881f,2.4000f, 1.0000f);
	public static 			DiSegment		OutQuintPoly	=> new(0.0000f, 2.4000f,0.0881f, 0.9700f, 0.0881f,0.0000f, 1.0000f);
	public static 			TetraSegment 	InOutQuintPoly	=> new(0.0000f, 0.0000f,0.0500f, 0.0157f, 0.0500f,1.1851f, 0.5000f, 1.1851f,0.0500f, 0.9843f, 0.0500f,0.0000f, 1.0000f);
	
	public static  			TetraSegment	InExpoPoly		=> new(0.0000f, 0.0000f,0.0100f, 0.0055f, 0.0100f,0.0500f, 0.0314f, 0.0500f,0.2740f, 0.1730f, 0.2740f,1.6745f, 1.0000f);
	public static  			TetraSegment	OutExpoPoly		=> new(0.0000f, 1.6745f,0.2740f, 0.8270f, 0.2740f,0.0500f, 0.9686f, 0.0500f,0.0100f, 0.9945f, 0.0100f,0.0000f, 1.0000f);
	public static  			TetraSegment	InOutExpoPoly	=> new(0.0000f, 0.0000f,0.0400f, 0.0153f, 0.0400f,1.4000f, 0.5000f, 1.4000f,0.0400f, 0.9847f, 0.0400f,0.0000f, 1.0000f);
	
	public static 			PentaSegment 	InCircPoly		=> new(0.0000f, 0.0000f,0.0415f, 0.0205f, 0.0415f,0.0856f, 0.0827f, 0.0856f,0.1458f, 0.2004f, 0.1458f,0.2700f, 0.4012f, 0.2700f,1.3201f, 1.0000f);
	public static 			TetraSegment 	OutCircPoly		=> new(0.0000f, 1.4500f,0.2799f, 0.6627f, 0.2799f,0.1400f, 0.8656f, 0.1400f,0.0660f, 0.9679f, 0.0660f,0.0000f, 1.0000f);
	public static 			OctaSegment 	InOutCircPoly	=> new(0.0000f, 0.0000f,0.0317f, 0.0158f, 0.0317f,0.0741f, 0.0673f, 0.1232f,0.3837f, 0.2820f, 0.1039f,0.4600f, 0.5000f, 0.4600f,0.1039f, 0.7179f, 0.3837f,0.1232f, 0.9332f, 0.0741f,0.0317f, 0.9839f, 0.0317f,0.0000f, 1.0000f, 0.125f, 0.250f, 0.450f, 0.500f, 0.550f, 0.750f, 0.875f);
	
	public static 			TetraSegment 	InBackPoly		=> new(0.0000f, 0.0000f,-0.0804f, -0.0636f, -0.0804f,0.0854f, -0.0879f, 0.0854f,0.4941f, 0.1817f, 0.4941f,1.1716f, 1.0000f);
	public static 			TetraSegment 	OutBackPoly		=> new(0.0000f, 1.1647f,0.4987f, 0.8173f, 0.4987f,0.0809f, 1.0875f, 0.0809f,-0.0864f, 1.0640f, -0.0864f,0.0000f, 1.0000f);
	public static 			TetraSegment 	InOutBackPoly	=> new(0.0000f, 0.0100f,0.0265f, -0.1000f, 0.0265f,1.4000f, 0.5000f, 1.4000f,0.0265f, 1.1000f, 0.0265f,0.0100f, 1.0000f);
	
	public static 			HexaSegment 	InElasticPoly	=> new(0.0000f, 0.0387f,0.0690f, 0.0009f, 0.0452f,-0.1381f, -0.0221f, -0.1060f,0.3358f, 0.0446f, 0.3624f,-1.0000f, -0.1359f, -0.8500f,2.5000f, 0.2501f, 0.6543f,0.3928f, 1.0000f, 0.330f, 0.510f, 0.650f, 0.802f, 0.943f);
	public static 			PentaSegment 	OutElasticPoly	=> new(0.0000f, 4.2593f,-1.4210f, 1.1408f, -0.9362f,0.4577f, 0.9587f, 0.3834f,-0.1089f, 1.0090f, -0.1814f,-0.0251f, 1.0053f, -0.0168f,0.0387f, 1.0000f, 0.200f, 0.350f, 0.510f, 0.760f);
	public static 			OctaSegment 	InOutElasticPoly=> new(0.0000f, 0.0093f,-0.0141f, -0.0018f, -0.0132f,0.0571f, 0.0098f, 0.0565f,-0.2771f, -0.0514f, -0.3096f,2.2000f, 0.5000f, 2.2000f,-0.3096f, 1.0514f, -0.2771f,0.0609f, 0.9901f, 0.0587f,-0.0152f, 1.0016f, -0.0116f,0.0060f, 1.0000f, 0.130f, 0.245f, 0.360f, 0.500f, 0.640f, 0.755f, 0.870f);
	
	public static 			TetraSegment 	InBouncePoly	=> new(0.0000f, 0.0600f,-0.0600f, 0.0000f, 0.2500f,-0.2500f, 0.0006f, 1.0000f,-1.0000f, 0.0000f, 2.0000f,-0.0136f, 1.0000f, 0.090f, 0.275f, 0.637f);
	public static 			TetraSegment 	OutBouncePoly	=> new(0.0000f, 0.0000f,2.0000f, 1.0000f, -1.0000f,1.0000f, 1.0000f, -0.2500f,0.2500f, 1.0000f, -0.0620f,0.0620f, 1.0000f, 0.364f, 0.728f, 0.909f);
	public static 			OctaSegment 	InOutBouncePoly	=> new(0.0000f, 0.0300f,-0.0300f, 0.0000f, 0.1250f,-0.1250f, 0.0000f, 0.5000f,-0.5000f, 0.0000f, 1.0000f,0.0000f, 0.5000f, 0.0000f,1.0000f, 1.0000f, -0.5000f,0.5000f, 1.0000f, -0.1250f,0.1250f, 1.0000f, -0.0300f,0.0300f, 1.0000f, 0.045f, 0.136f, 0.318f, 0.500f, 0.682f, 0.864f, 0.955f);
	
	public static 			TetraSegment 	SqrtPoly		=> new(0.0000f, 0.2204f,0.0518f, 0.0997f, 0.3713f,0.1515f, 0.3172f, 0.3926f,0.2203f, 0.6021f, 0.5096f,0.3136f, 1.0000f, 0.010f, 0.100f, 0.360f);
}