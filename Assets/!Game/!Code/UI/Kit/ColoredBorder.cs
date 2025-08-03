namespace Flexy.Template.BarleyBreak.UI
{
	public class ColoredBorder : UIWidgetEx
	{
		[Bindable]	Color	BorderColor		=> Game.Settings.SettingsTabColor.Primary;
	
		private		void	OnEnable		( ) => Game.Settings.SettingsTabColor.Primary.Changed += RebindColor;
		private		void	OnDisable		( ) => Game.Settings.SettingsTabColor.Primary.Changed -= RebindColor;

		private		void	RebindColor		( Color32 color ) => RebindProperty( "BorderColor" );
	}
}