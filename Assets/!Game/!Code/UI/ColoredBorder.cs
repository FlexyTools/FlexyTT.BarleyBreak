namespace Flexy.Template.BarleyBreak.UI
{
	public class ColoredBorder : UIWidgetEx
	{
		[Bindable]	Color	BorderColor		=> Game.Settings.Color.Primary;
	
		private void	OnEnable	( ) => Game.Settings.Color.Primary.Changed += RebindColor;
		private void	OnDisable	( ) => Game.Settings.Color.Primary.Changed -= RebindColor;

		private void	RebindColor	( Color32 color ) => RebindProperty( "BorderColor" );
	}
}