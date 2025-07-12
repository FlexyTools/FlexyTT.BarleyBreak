// ReSharper disable AccessToStaticMemberViaDerivedType
namespace Flexy.Template.BarleyBreak.CoreGame;

public struct	Facade_Coregame
{
    public	GameContext			    Ctx;
													
    public	Facade_CoreUIWindows	UI				=> new(Ctx.GetService<GameStage>( ));
    public	GameSettingsService	    Settings		=> Ctx.GetService<GameSettingsService>( );
    public  GameMode				Mode         	=> Ctx.GetService<GameMode>( );
    public	Service_Leaderboards	Leaderboards	=> Ctx.GetService<Service_Leaderboards>( );

    public	Facade_Coregame			GetCached			( Component callSource ) => (GameContext.GetCached(ref Ctx, callSource), this).Item2;
}

public readonly record struct Facade_CoreUIWindows( FlowLib Lib )
{
	public Window_Pause				.Opener		Pause				=> Lib.GetState<Window_Pause>( );
	public Window_FieldComplete		.Opener		FieldComplete		=> Lib.GetOpener<Window_FieldComplete.Opener>( );
	public Window_Leaderboard		.Opener		Leaderboards		=> Lib.GetOpener<Window_Leaderboard.Opener>( );
	public Window_GameSettings		.Opener		GameSettings		=> Lib.GetState<Window_GameSettings>( );
}