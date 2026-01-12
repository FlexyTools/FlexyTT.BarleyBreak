// ReSharper disable AccessToStaticMemberViaDerivedType

using FlexyTT.BarleyBreak.Metagame.Leaderboards;
using FlexyTT.BarleyBreak.Common;
using FlexyTT.BarleyBreak.Coregame.Kit;
using FlexyTT.BarleyBreak.Metagame;

namespace FlexyTT.BarleyBreak.Coregame;

public struct	Facade_Coregame : ICachedContext
{
	public	GameContext				Ctx				{ get; set; }
	public	Component				CallSource		{ get; set; }
													
	public  GameMode				Mode         	=> Ctx.GetService<GameMode>();
	public	Facade_Flow				Flow 			=> new(Ctx.GetService<Stage_Coregame>());
    public	Facade_CoreStates		States			=> new(CallSource.GetComponentInParent<State>());
    public	Service_Audio			Audio			=> Ctx.GetService<Service_Audio>();
    public	Service_GameSettings	Settings		=> Ctx.GetService<Service_GameSettings>();
}

public readonly record struct	Facade_Flow	( Stage_Coregame Core )
{
	public	void	ExitCoregame	( )		=> Core.Exit();
}

public readonly record struct	Facade_CoreStates	( LibCtx LibCtx )
{
	public State_Play				.Opener		Play				=> LibCtx.GetState<State_Play>();
	public State_Pause				.Opener		Pause				=> LibCtx.GetState<State_Pause>();
	public State_PlayComplete		.Opener		PlayComplete		=> LibCtx.GetOpener<State_PlayComplete.Opener>();
	public Window_GameSettings		.Opener		GameSettings		=> LibCtx.GetState<Window_GameSettings>();
}