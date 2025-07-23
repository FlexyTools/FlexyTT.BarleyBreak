// ReSharper disable AccessToStaticMemberViaDerivedType
namespace Flexy.Template.BarleyBreak;

public struct	Facade_Game
{
	public	GameContext	Ctx;

	public	Facade_UIWindows		UI				=> new( Ctx.GetService<GameStage>( ) );
	public	Facade_GameSettings		Settings		=> new( Ctx.GetService<GameSettingsService>( ) );
	public	Service_Leaderboards	Leaderboards	=> Ctx.GetService<Service_Leaderboards>( );

	public	Facade_Game				GetCached		( Component callSource ) => (GameContext.GetCached(ref Ctx, callSource), this).Item2; 
}

public record struct	Facade_GameSettings ( GameSettingsService Svc )
{
	public	AudioSettingsTab		Audio	=> Svc.Get<AudioSettingsTab>( );
	public	ColorSettingsTab		Color	=> Svc.Get<ColorSettingsTab>( );
}

public record struct	Facade_UIWindows	( FlowLib Lib )
{
	private const String CoreGameStage = "d1be6da70d122814e85788d63b8276bd";

	public Window_GameSettings		.Opener		Settings			=> Lib.GetState<Window_GameSettings>( );
	public Window_PlayFields		.Opener		PlayFields			=> Lib.GetState<Window_PlayFields>( );
	public Window_Leaderboard		.Opener		Leaderboards		=> Lib.GetOpener<Window_Leaderboard.Opener>( );
	
	public ResultStateHandle<Single> Play_Field	( SceneRef map )	=> Lib.FlowSvc.SpawnGameStage( new( CoreGameStage ), map );
}