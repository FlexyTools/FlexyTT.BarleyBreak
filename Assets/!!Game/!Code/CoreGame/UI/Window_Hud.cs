namespace Flexy.Template.BarleyBreak.CoreGame.UI
{
    public class Window_Hud : UIWindowEx
    {
		[Bindable]			Int32		RunMinutes		=> TimeSpan.FromSeconds( Game.Mode.RunTime ).Minutes;
		[Bindable]			Int32		RunSeconds		=> TimeSpan.FromSeconds( Game.Mode.RunTime ).Seconds;
		[Bindable]			Int32		RunMiliseconds	=> TimeSpan.FromSeconds( Game.Mode.RunTime ).Milliseconds;
		
		protected override void		OnBackShow	( )		
		{
			Time.timeScale = 1;
		}
		protected override Boolean	TryGoBack	( )		
		{
			Pause( );
			return false;
		}

		private			void		OnApplicationPause	( Boolean pauseStatus )	
		{
			if( !Application.isEditor )
				Game.UI.Pause.Open( );
		}
		
        [Callable] void Pause		( )			
        {
			Game.UI.Pause.Open( );
        }
		[Callable] void OpenFreeCam	( )			
		{
			Game.UI.FreeCam.Open( );
		}
    }
}