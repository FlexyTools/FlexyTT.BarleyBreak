namespace Test.Runner.CoreGame.UI
{
	public class Window_GameOver : UIWindowEx
    {
	    [SerializeField]	AudioItem	_failSong;
	    
        [Bindable]		Single		Seconds				=> (Single)OpenParams;
        [Bindable]		String		FormattedSeconds	=> TimeSpan.FromSeconds( Seconds ).ToString( "g" );

        protected override	Boolean	TryGoBack	( )		=> false;

        protected override void		OnShow		( )	
        {
	        Game.AudioPlayer.StopPlaylist( );
	        _failSong.PlayOneShot( );
	        
	        base.OnShow();
        }
        protected override void		OnHide		( )	
        {
	        Game.AudioPlayer.PlayPlaylist( );
	        base.OnHide( );
        }

        [Callable]		void		Continue	( )	
		{
			Game.AudioPlayer.PlayPlaylist( );
			GameStage.CloseStage( );
		}
        
		public record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	StateHandle		Open	( Single score ) => Ctx.Open( score );
		}
    }
}