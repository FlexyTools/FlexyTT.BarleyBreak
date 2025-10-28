// ReSharper disable AccessToStaticMemberViaDerivedType

using FlexyTemplates.BarleyBreak.Coregame.States;
using FlexyTemplates.BarleyBreak.Metagame;
using FlexyTemplates.BarleyBreak.Metagame.Leaderboards;

namespace FlexyTemplates.BarleyBreak.Coregame;

public struct	Facade_Coregame : ICachedContext
{
	public	GameContext				Ctx				{ get; set; }
	public	Component				CallSource		{ get; set; }
													
	public  GameMode				Mode         	=> Ctx.GetService<GameMode>()!;
    public	Facade_CoreStates		States			=> new(CallSource.GetComponentInParent<State>());
    public	Service_Audio			Audio			=> Ctx.GetService<Service_Audio>();
    public	Service_GameSettings	Settings		=> Ctx.GetService<Service_GameSettings>();
    public	Service_Leaderboards	Leaderboards	=> Ctx.GetService<Service_Leaderboards>();
}

public readonly record struct Facade_CoreStates( LibCtx LibCtx )
{
	public State_Pause				.Opener		Pause				=> LibCtx.GetState<State_Pause>();
	public State_PlayComplete		.Opener		FieldComplete		=> LibCtx.GetOpener<State_PlayComplete.Opener>();
	public Window_Leaderboards		.Opener		Leaderboards		=> LibCtx.GetOpener<Window_Leaderboards.Opener>();
	public Window_GameSettings		.Opener		GameSettings		=> LibCtx.GetState<Window_GameSettings>();
}