using FlexyFun.BarleyBreak.Overlays;

namespace FlexyTT.BarleyBreak.Global;

public readonly record struct	Facade_Overlay	( Component src )
{
	public readonly LibCtx LibCtx	= src.GetComponentInParent<State>().Flow;
	
	public Overlay_NoInternet		.Opener		NoInternet			=> LibCtx.GetState<Overlay_NoInternet>();
}