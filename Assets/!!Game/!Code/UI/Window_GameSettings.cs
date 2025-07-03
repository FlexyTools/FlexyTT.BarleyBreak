namespace Runner.UI
{
	public class Window_GameSettings : UIWindowEx
	{
		private AudioSettingsTab _audioSettingsTab;

		[Bindable]	Single	MusicVolume		 
		{
			get => _audioSettingsTab.MusicVolume;
			set { _audioSettingsTab.MusicVolume.Set( value ); RebindProperty( "MusicVolume", "MusicVolume_100" ); }
		}
		[Bindable]	Int32	MusicVolume_100 => (Int32)(MusicVolume * 100);
		
		[Bindable]	Single	SfxVolume		
		{
			get => _audioSettingsTab.SfxVolume;
			set { _audioSettingsTab.SfxVolume.Set( value ); RebindProperty( "SfxVolume", "SfxVolume_100" ); }
		}
		[Bindable]	Int32	SfxVolume_100	=> (Int32)(SfxVolume * 100);
		
		private		void	Awake	( )		
		{
			_audioSettingsTab = Game.Settings.Get<AudioSettingsTab>( );
		}
	}
}

