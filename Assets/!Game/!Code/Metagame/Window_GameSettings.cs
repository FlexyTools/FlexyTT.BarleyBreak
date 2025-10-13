namespace FlexyTemplates.BarleyBreak.Metagame
{
	public class Window_GameSettings : UIWindowEx
	{
		private		void	Awake	( )		
		{
			_settingsTabAudio = Game.Settings.Audio;
			_settingsTabColor = Game.Settings.Color;
			
			_settingsTabColor.Primary.Changed += _ => RebindProperty( "Color" );
		}
	
		private SettingsTab_Audio _settingsTabAudio;
		private SettingsTab_Color _settingsTabColor;

		// Audio Settings
		[Bindable]	Single	SoundVolume		
		{ 
			get => _settingsTabAudio.SoundVolume; 
			set 
			{ 
				_settingsTabAudio.SoundVolume.Set( value ); 
				RebindProperty( "SoundVolume" ); 
				RebindProperty( "SoundVolume_100" ); 
			} 
		}
		[Bindable]	Single	SfxVolume		
		{ 
			get => _settingsTabAudio.SfxVolume;  
			set 
			{
				_settingsTabAudio.SfxVolume.Set( value ); 
				RebindProperty( "SfxVolume" ); 
				RebindProperty( "SfxVolume_100" ); 
			} 
		}
		
		[Bindable]	String	SoundVolume_100 => ((Int32)(SoundVolume * 100)).ToString();
		[Bindable]	String	SfxVolume_100	=> ((Int32)(SfxVolume * 100)).ToString();

		// Color Settings
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
	}
} 