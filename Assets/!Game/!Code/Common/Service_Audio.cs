namespace FlexyTemplates.BarleyBreak.Common
{
	public class Service_Audio : MonoBehaviour, IService
	{
		[SerializeField]	AudioSource		_soundSource = null!;
		[SerializeField]	AudioSource		_sfxSource = null!;
		[SerializeField]	AudioMixer		_mixer = null!;
		
		[Header("Sound Meta vs Core")]
		[SerializeField]	Single	_soundMetaVolume	= 0.5f;
		[SerializeField]	Single	_soundMetaPitch		= 0.8f;
		[SerializeField]	Single	_soundCoreVolume	= 1f;
		[SerializeField]	Single	_soundCorePitch		= 1.1f;
		
		private SettingsTab_Audio _settings = null!;

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
			_settings = ctx.GetService<Service_GameSettings>().Get<SettingsTab_Audio>();
			
			_settings.SoundVolume	.Changed += _ => UpdateVolume();
			_settings.SfxVolume		.Changed += _ => UpdateVolume();
			
			_soundSource.volume	= _soundMetaVolume;
			_soundSource.pitch	= _soundMetaPitch;
		}
		public	void	PlaySfx			( AudioClip clip )	
		{
			_sfxSource.PlayOneShot(clip);
		}
		public	void	SwitchToCore	( )					
		{
			_soundSource.Stop();
			_soundSource.volume	= _soundCoreVolume;
			_soundSource.pitch	= _soundCorePitch;
			_soundSource.PlayDelayed(0.1f);
		}
		public	void	SwitchToMeta	( )					
		{
			_soundSource.Stop();
			_soundSource.volume	= _soundMetaVolume;
			_soundSource.pitch	= _soundMetaPitch;
			_soundSource.PlayDelayed(0.1f);
		}
		private	void	UpdateVolume	( )					
		{
			SoundVolume	= _settings.SoundVolume;
			SfxVolume	= _settings.SfxVolume; 
		}
		
		private static 	Single 	DbToLinear	( Single db )		=> Mathf.Pow( 10f, db / 20f );
		private static 	Single 	LinearToDb	( Single linear )	=> linear <= 0f ? -80 : Mathf.Log10( linear ) * 20;
	}
	
	public class PlaySfxAction : FlexyActionSync
	{
		[SerializeField]	AudioClip	_clip = null!;
	
		public override void Do	( ActionCtx ctx )	
		{
			ctx.SrcObject.GetService<Service_Audio>().PlaySfx( _clip );
		}
	}
}