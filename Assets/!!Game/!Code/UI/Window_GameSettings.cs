using Flexy.Template.BarleyBreak.Settings;

namespace Flexy.Template.BarleyBreak.UI
{
	public class Window_GameSettings : UIWindowEx
	{
		private ColorSettingsTab _colorSettingsTab;

		[Bindable]	Color	PrimaryColor		 
		{
			get => _colorSettingsTab.PrimaryColor.Get();
			set { _colorSettingsTab.PrimaryColor.Set( value ); RebindProperty( "PrimaryColor" ); }
		}
		//[Bindable]	Int32	MusicVolume_100 => (Int32)(MusicVolume * 100);
		
		[Bindable]	Color	SecondaryColor		
		{
			get => _colorSettingsTab.SecondaryColor.Get();
			set { _colorSettingsTab.SecondaryColor.Set( value ); RebindProperty( "SecondaryColor" ); }
		}
		//[Bindable]	Int32	SfxVolume_100	=> (Int32)(SfxVolume * 100);
		
		private		void	Awake	( )		
		{
			_colorSettingsTab = Game.Settings.Get<ColorSettingsTab>( );
		}
	}
}