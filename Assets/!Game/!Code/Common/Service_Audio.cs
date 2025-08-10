namespace Flexy.Template.BarleyBreak.Common
{
	public class Service_Audio : MonoBehaviour, IService
	{
		[SerializeField]	AudioSource		_soundSource;
		[SerializeField]	AudioSource		_sfxSource;
		[SerializeField]	AudioMixer		_mixer;
		
		private SettingsTab_Audio _settings;

		public	Single	SoundVolume		
		{
			get => _mixer.GetFloat( "SoundVolume",  out var volume ) ? DbToLinear(volume) : 0;
			set => _mixer.SetFloat( "SoundVolume",  LinearToDb(value) );
		}
		public	Single	SfxVolume		
		{
			get => _mixer.GetFloat( "SfxVolume",  out var volume ) ? DbToLinear(volume) : 0;
			set => _mixer.SetFloat( "SfxVolume",  LinearToDb(value) );
		}

		public	void	OrderedInit		( GameContext ctx )	
		{
			_settings = ctx.GetService<Service_GameSettings>( ).Get<SettingsTab_Audio>( );
			
			_settings.SoundVolume	.Changed += _ => UpdateVolume( );
			_settings.SfxVolume		.Changed += _ => UpdateVolume( );
		}
		public	void	PlaySfx			( AudioClip clip )	
		{
			_sfxSource.PlayOneShot( clip );
		}
		private	void	UpdateVolume	( )					
		{
			SoundVolume	= _settings.SoundVolume;
			SfxVolume	= _settings.SfxVolume; 
		}
		
		private static 	Single 	DbToLinear	( Single db )		=> Mathf.Pow(10f, db / 20f);
		private static 	Single 	LinearToDb	( Single linear )	=> linear <= 0f ? -80 : Mathf.Log10( linear )*20;
	}
	
	public class PlaySfxAction : FlexyActionSync
	{
		[SerializeField]	AudioClip	_clip;
	
		public override void Do	( ActionCtx ctx )	
		{
			ctx.CtxObj.gameObject.GetService<Service_Audio>( ).PlaySfx( _clip );
		}
	}
}