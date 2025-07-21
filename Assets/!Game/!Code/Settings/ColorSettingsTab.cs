namespace Flexy.Template.BarleyBreak.Settings;

public class ColorSettingsTab : GameSettingsTab
{
	public ColorSetting		Primary		= new( "ColorSettingsTab_Primary",	ColorUtility.TryParseHtmlString("#EC1762", out var color) ? color : Color.gray );
}