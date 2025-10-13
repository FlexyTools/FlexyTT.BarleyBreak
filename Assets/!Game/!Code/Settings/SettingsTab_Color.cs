namespace FlexyTemplates.BarleyBreak.Settings;

public class SettingsTab_Color : GameSettingsTab
{
	public ColorSetting		Primary		= new( "ColorSettingsTab_Primary",	ColorUtility.TryParseHtmlString("#2E7474", out var color) ? color : Color.gray );
}