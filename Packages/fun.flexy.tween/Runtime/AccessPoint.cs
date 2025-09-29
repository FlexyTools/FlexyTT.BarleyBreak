namespace Flexy.Tweens;

public struct TweenAccess 
{
	internal Service_Tween	Svc;
}

public static class AccessPoint
{
	public static TweenAccess Tween => new() {Svc = Service_Tween.Global};
}