namespace Flexy.Template.BarleyBreak.Settings;

public class AudioSettingsTab : GameSettingsTab
{
	public SingleSetting	SoundVolume		= new( "AudioSettingsTab_SoundVolume",		1 );
	public SingleSetting	SfxVolume		= new( "AudioSettingsTab_SfxVolume",		1 );
}

public class ColorSettingsTab : GameSettingsTab
{
	public ColorSetting		Primary			= new( "ColorSettingsTab_Primary",			ColorUtility.TryParseHtmlString("#EC1762", out var color) ? color : Color.gray );
}