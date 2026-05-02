// ReSharper disable AccessToStaticMemberViaDerivedType

using FlexyTT.BarleyBreak.Menu.Leaderboards;
using FlexyTT.BarleyBreak.Menu.Settings;

namespace FlexyTT.BarleyBreak.Menu;

public struct	Facade_Game : ICachedContext
{
	public	GameContext				Ctx				{ get; set; }
	public	Component				CallSource		{ get; set; }

	public	Facade_Flow				Flow			=> new( Ctx.GetService<GameStage_Menu>() );
	public	Facade_UIWindows		UI				=> new( CallSource );
	public	Facade_GameSettings		Settings		=> new( Ctx.GetService<Service_GameSettings>() );
	public	Service_Leaderboards	Leaderboards	=> Ctx.GetService<Service_Leaderboards>();
}

public record struct	Facade_GameSettings ( Service_GameSettings Svc )
{
	public	SettingsTab_Audio		Audio		=> Svc.Get<SettingsTab_Audio>();
	public	SettingsTab_Color		Color		=> Svc.Get<SettingsTab_Color>();
}

public readonly record struct	Facade_Flow	( GameStage_Menu Meta )
{
	public	void	PlayField	( SceneRef map )	=> Meta.Play_Field(map).Forget();
}

public readonly record struct	Facade_UIWindows	( LibCtx LibCtx )
{
	public Window_GameSettings		.Opener		Settings		=> LibCtx.GetState<Window_GameSettings>();
	public Tab_GameSettings_Audio	.Opener		Settings_Audio	=> LibCtx.GetState<Tab_GameSettings_Audio>();
	public Tab_GameSettings_Color	.Opener		Settings_Color	=> LibCtx.GetState<Tab_GameSettings_Color>();

	public Window_AppInfo			.Opener		AppInfo			=> LibCtx.GetState<Window_AppInfo>();
	public Window_PlayFields		.Opener		PlayFields		=> LibCtx.GetState<Window_PlayFields>();
	public Window_Leaderboards		.Opener		Leaderboards	=> LibCtx.GetOpener<Window_Leaderboards.Opener>();
}