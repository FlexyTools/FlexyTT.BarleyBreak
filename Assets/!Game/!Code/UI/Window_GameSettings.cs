using Flexy.Template.BarleyBreak.Settings;

namespace Flexy.Template.BarleyBreak.UI
{
	public class Window_GameSettings : UIWindowEx
	{
		private		void	Awake	( )		
		{
			_audioSettingsTab = Game.Settings.Svc.Get<AudioSettingsTab>( );
			_colorSettingsTab = Game.Settings.Svc.Get<ColorSettingsTab>( );
			
			_colorSettingsTab.Primary.Changed += _ => RebindProperty( "Color" );
		}
	
		private AudioSettingsTab _audioSettingsTab;
		private ColorSettingsTab _colorSettingsTab;

		// Audio Settings
		[Bindable]	Single	SoundVolume		
		{ 
			get => _audioSettingsTab.SoundVolume; 
			set 
			{ 
				_audioSettingsTab.SoundVolume.Set( value ); 
				RebindProperty( "SoundVolume" ); 
				RebindProperty( "SoundVolume_100" ); 
			} 
		}
		[Bindable]	Single	SfxVolume		
		{ 
			get => _audioSettingsTab.SfxVolume;  
			set 
			{
				_audioSettingsTab.SfxVolume.Set( value ); 
				RebindProperty( "SfxVolume" ); 
				RebindProperty( "SfxVolume_100" ); 
			} 
		}
		
		[Bindable]	Int32	SoundVolume_100 => (Int32)(SoundVolume * 100);
		[Bindable]	Int32	SfxVolume_100	=> (Int32)(SfxVolume * 100);

		// Color Settings
		[Bindable]	Single	ColorR	
		{
			get => _colorSettingsTab.Primary.Get().r / 255f;
			set 
			{ 
				_colorSettingsTab.Primary.Set( _colorSettingsTab.Primary.Get() with {r = (Byte)(value*255)} ); 
				RebindProperty( "ColorR" ); 
				RebindProperty( "ColorR_255" ); 
			}
		}
		[Bindable]	Single	ColorG	
		{
			get => _colorSettingsTab.Primary.Get().g / 255f;
			set 
			{ 
				_colorSettingsTab.Primary.Set( _colorSettingsTab.Primary.Get() with {g = (Byte)(value*255)} ); 
				RebindProperty( "ColorG" ); 
				RebindProperty( "ColorG_255" ); 
			}
		}
		[Bindable]	Single	ColorB	
		{
			get => _colorSettingsTab.Primary.Get().b / 255f;
			set 
			{
				_colorSettingsTab.Primary.Set( _colorSettingsTab.Primary.Get() with {b = (Byte)(value*255)} ); 
				RebindProperty( "ColorB" ); 
				RebindProperty( "ColorB_255" ); 
			}
		}
		
		[Bindable]	Int32	ColorR_255 => (Int32)(ColorR * 255);
		[Bindable]	Int32	ColorG_255 => (Int32)(ColorG * 255);
		[Bindable]	Int32	ColorB_255 => (Int32)(ColorB * 255);
		
		[Bindable]	Color	Color		=> _colorSettingsTab.Primary.Get();
	}
} 