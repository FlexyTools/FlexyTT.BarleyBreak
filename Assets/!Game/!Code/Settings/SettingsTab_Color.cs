namespace FlexyTT.BarleyBreak.Settings;

public class SettingsTab_Color : GameSettingsTab
{
	public ColorSetting		Primary		= new( "ColorSettingsTab_Primary",	ColorUtility.TryParseHtmlString("#8F48AC", out var color) ? color : Color.gray );
}