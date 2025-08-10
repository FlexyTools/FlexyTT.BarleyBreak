// ReSharper disable AccessToStaticMemberViaDerivedType
namespace Flexy.Template.BarleyBreak.CoreGame;

public struct	Facade_Coregame : ICachedContext
{
	public	GameContext				Ctx				{ get; set; }
	public	Component				CallSource		{ get; set; }
													
	public  GameMode				Mode         	=> Ctx.GetService<GameMode>();
    public	Facade_CoreStates		States			=> new(CallSource.GetComponentInParent<State>());
    public	Service_GameSettings	Settings		=> Ctx.GetService<Service_GameSettings>();
    public	Service_Leaderboards	Leaderboards	=> Ctx.GetService<Service_Leaderboards>();
}

public readonly record struct Facade_CoreStates( LibCtx LibCtx )
{
	public State_Pause				.Opener		Pause				=> LibCtx.GetState<State_Pause>();
	public State_FieldComplete		.Opener		FieldComplete		=> LibCtx.GetOpener<State_FieldComplete.Opener>();
	public Window_Leaderboard		.Opener		Leaderboards		=> LibCtx.GetOpener<Window_Leaderboard.Opener>();
	public Window_GameSettings		.Opener		GameSettings		=> LibCtx.GetState<Window_GameSettings>();
}