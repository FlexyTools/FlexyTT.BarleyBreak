// ReSharper disable AccessToStaticMemberViaDerivedType

using FlexyTemplates.BarleyBreak.Metagame;
using FlexyTemplates.BarleyBreak.Metagame.Leaderboards;

namespace FlexyTemplates.BarleyBreak;

public struct	Facade_Game : ICachedContext
{
	public	GameContext				Ctx				{ get; set; }
	public	Component				CallSource		{ get; set; }

	public	BarleyBreak_GameFlow	Flow			=> Ctx.GetService<BarleyBreak_GameFlow>();
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
	public Window_GameSettings		.Opener		Settings		=> LibCtx.GetState<Window_GameSettings>();
	public Window_AppInfo			.Opener		AppInfo			=> LibCtx.GetState<Window_AppInfo>();
	public Window_PlayFields		.Opener		PlayFields		=> LibCtx.GetState<Window_PlayFields>();
	public Window_Leaderboards		.Opener		Leaderboards	=> LibCtx.GetOpener<Window_Leaderboards.Opener>();
}