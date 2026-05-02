using FlexyFun.BarleyBreak.Overlays;

namespace FlexyTT.BarleyBreak.Global;

public readonly record struct	Facade_Global	( LibCtx LibCtx )
{
	public Overlay_NoInternet		.Opener		NoInternet			=> LibCtx.GetState<Overlay_NoInternet>();
}