using FlexyTT.BarleyBreak.Menu;
using FlexyTT.BarleyBreak.Settings;

namespace FlexyTT.BarleyBreak.Common.Kit
{
	public class Widget_ColoredBorder : UIWidgetEx
	{
		[SerializeField]	Color			_tintColor		= Color.white;
		
		private		SettingsTab_Color		_colorSettings	= null!;
		
		[Bindable]	Color	BorderColor		=> _colorSettings.Primary * _tintColor;
	
		private		void	OnEnable		( ) => (_colorSettings = Game.Settings.Color).Primary.Changed += RebindColor;
		private		void	OnDisable		( ) => _colorSettings.Primary.Changed -= RebindColor;

		private		void	RebindColor		( Color32 color ) => RebindProperty( "BorderColor" );
	}
}