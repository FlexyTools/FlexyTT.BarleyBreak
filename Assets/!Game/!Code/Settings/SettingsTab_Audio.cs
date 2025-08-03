namespace Flexy.Template.BarleyBreak.Settings;

public class SettingsTab_Audio : GameSettingsTab
{
	public SingleSetting	SoundVolume		= new( "AudioSettingsTab_SoundVolume",	1 );
	public SingleSetting	SfxVolume		= new( "AudioSettingsTab_SfxVolume",	1 );
}