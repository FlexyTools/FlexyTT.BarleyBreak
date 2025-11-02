using FlexyTemplates.BarleyBreak.Metagame;

namespace FlexyTemplates.BarleyBreak.Common.Kit
{
	public class Widget_ColoredBorder : UIWidgetEx
	{
		private		SettingsTab_Color		_colorSettings	= null!;
		[Bindable]	Color	BorderColor		=> _colorSettings.Primary;
	
		private		void	OnEnable		( ) => (_colorSettings = Game.Settings.Color).Primary.Changed += RebindColor;
		private		void	OnDisable		( ) => _colorSettings.Primary.Changed -= RebindColor;

		private		void	RebindColor		( Color32 color ) => RebindProperty( "BorderColor" );
	}
}