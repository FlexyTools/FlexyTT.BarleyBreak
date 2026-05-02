namespace FlexyTT.BarleyBreak.Menu.Settings
{
	[OpenAsSubStateIn(typeof(Window_GameSettings))]
	public class Tab_GameSettings_Audio : UIWindowEx
	{
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
	
		private		void	Awake	( )		
		{
			_settingsTabAudio = Game.Settings.Audio;
		}
	
		private SettingsTab_Audio _settingsTabAudio = null!;
	}
}