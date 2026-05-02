namespace FlexyTT.BarleyBreak.Menu.Settings
{
	[OpenAsSubStateIn(typeof(Window_GameSettings))]
	public class Tab_GameSettings_Color : UIWindowEx
	{
		[Bindable]	Single	ColorR			
		{
			get => _settingsTabColor.Primary.Get().r / 255f;
			set 
			{ 
				_settingsTabColor.Primary.Set( _settingsTabColor.Primary.Get() with {r = (Byte)(value*255)} ); 
				RebindProperty( "ColorR" ); 
				RebindProperty( "ColorR_255" ); 
			}
		}
		[Bindable]	Single	ColorG			
		{
			get => _settingsTabColor.Primary.Get().g / 255f;
			set 
			{ 
				_settingsTabColor.Primary.Set( _settingsTabColor.Primary.Get() with {g = (Byte)(value*255)} ); 
				RebindProperty( "ColorG" ); 
				RebindProperty( "ColorG_255" ); 
			}
		}
		[Bindable]	Single	ColorB			
		{
			get => _settingsTabColor.Primary.Get().b / 255f;
			set 
			{
				_settingsTabColor.Primary.Set( _settingsTabColor.Primary.Get() with {b = (Byte)(value*255)} ); 
				RebindProperty( "ColorB" ); 
				RebindProperty( "ColorB_255" ); 
			}
		}
		
		[Bindable]	String	ColorR_255 		=> ((Int32)(ColorR * 255)).ToString();
		[Bindable]	String	ColorG_255 		=> ((Int32)(ColorG * 255)).ToString();
		[Bindable]	String	ColorB_255 		=> ((Int32)(ColorB * 255)).ToString();
		
		[Bindable]	Color	Color			=> _settingsTabColor.Primary.Get();
	
		private	new	void	Awake	( )		
		{
			base.Awake();
			_settingsTabColor = Game.Settings.Color;
		}
	
		private SettingsTab_Color _settingsTabColor = null!;	
	}
}