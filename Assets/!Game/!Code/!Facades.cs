// ReSharper disable AccessToStaticMemberViaDerivedType

using Flexy.Template.BarleyBreak.UI.Leaderboards;

namespace Flexy.Template.BarleyBreak;

public struct	Facade_Game : ICachedContext
{
	public	GameContext				Ctx				{ get; set; }
	public	Component				CallSource		{ get; set; }

	public	Facade_UIWindows		UI				=> new( Ctx.GetService<GameStage>() );
	public	Facade_GameSettings		Settings		=> new( Ctx.GetService<Service_GameSettings>() );
	public	Service_Leaderboards	Leaderboards	=> Ctx.GetService<Service_Leaderboards>();
}

public record struct	Facade_GameSettings ( Service_GameSettings Svc )
{
	public	SettingsTab_Audio		Audio		=> Svc.Get<SettingsTab_Audio>();
	public	SettingsTab_Color		Color		=> Svc.Get<SettingsTab_Color>();
}

public record struct	Facade_UIWindows	( LibCtx LibCtx )
{
	private const String CoreGameStage = "c1055f23b34e09a4496fc2c881bb0920";

	public Window_GameSettings		.Opener		Settings			=> LibCtx.GetState<Window_GameSettings>();
	public Window_PlayFields		.Opener		PlayFields			=> LibCtx.GetState<Window_PlayFields>();
	public Window_Leaderboards		.Opener		Leaderboards		=> LibCtx.GetOpener<Window_Leaderboards.Opener>();
	
	public void Play_Field	( SceneRef map )	=> LibCtx.Service.Graph.Open( new( CoreGameStage ), LibCtx.Src, map );
}