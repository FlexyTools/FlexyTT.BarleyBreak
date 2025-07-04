namespace Flexy.Template.BarleyBreak.Settings;

public class ColorSettingsTab : GameSettingsTab
{
	public ColorSetting		PrimaryColor	= new( "ColorSettingsTab_PrimaryColor",		Color.magenta );
	public ColorSetting		SecondaryColor	= new( "ColorSettingsTab_SecondaryColor",	Color.blue );
	public ColorSetting		AccentColor		= new( "ColorSettingsTab_AccentColor",		Color.black );
}