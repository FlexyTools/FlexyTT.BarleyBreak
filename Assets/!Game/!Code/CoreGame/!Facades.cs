// ReSharper disable AccessToStaticMemberViaDerivedType
namespace Flexy.Template.BarleyBreak.CoreGame;

public struct	Facade_Coregame : ICachedContext
{
	public	GameContext				Ctx				{ get; set; }
	public	Component				CallSource		{ get; set; }
													
    public	Facade_CoreUIWindows	UI				=> new(CallSource.GetComponentInParent<State>());
    public	GameSettingsService	    Settings		=> Ctx.GetService<GameSettingsService>( );
    public  GameMode				Mode         	=> Ctx.GetService<GameMode>( );
    public	Service_Leaderboards	Leaderboards	=> Ctx.GetService<Service_Leaderboards>( );
}

public readonly record struct Facade_CoreUIWindows( LibCtx LibCtx )
{
	public Window_Pause				.Opener		Pause				=> LibCtx.GetState<Window_Pause>( );
	public Window_FieldComplete		.Opener		FieldComplete		=> LibCtx.GetOpener<Window_FieldComplete.Opener>( );
	public Window_Leaderboard		.Opener		Leaderboards		=> LibCtx.GetOpener<Window_Leaderboard.Opener>( );
	public Window_GameSettings		.Opener		GameSettings		=> LibCtx.GetState<Window_GameSettings>( );
}