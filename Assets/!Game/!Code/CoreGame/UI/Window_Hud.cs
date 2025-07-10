namespace Flexy.Template.BarleyBreak.CoreGame.UI
{
    public class Window_Hud : UIWindowEx
    {
		[Bindable]			Int32		RunMinutes		=> TimeSpan.FromSeconds( Game.Mode.RunTime ).Minutes;
		[Bindable]			Int32		RunSeconds		=> TimeSpan.FromSeconds( Game.Mode.RunTime ).Seconds;
		[Bindable]			Int32		RunMiliseconds	=> TimeSpan.FromSeconds( Game.Mode.RunTime ).Milliseconds;

		private Boolean _finishingStarted;

		protected override void		OnShow		( )		
		{
			_finishingStarted = false;
		}
		protected override void		OnFwdHide	( )		
		{
			Time.timeScale = 0.001f;
		}
		protected override void		OnBackShow	( )		
		{
			Time.timeScale = 1;
		}
		protected override Boolean	TryGoBack	( )		
		{
			Pause( );
			return false;
		}

		private		void	Update				( )						
		{
			if (Game.Mode.IsWin && !_finishingStarted)
			{
				_finishingStarted = true;
				FinishGameAsync( ).Forget( );
			}	
		}
		private		void	OnApplicationPause	( Boolean pauseStatus )	
		{
			if( !Application.isEditor )
				Game.UI.Pause.Open( );
		}
		
        [Callable]	void	Pause				( )			
        {
			Game.UI.Pause.Open( );
        }
		
		private async	UniTaskVoid		FinishGameAsync	( )	
		{
			await UniTask.Delay( 1500, DelayType.UnscaledDeltaTime );
		
			Game.Leaderboards.AddRecord( Game.Mode.Board, Game.Mode.RunTime );

			Debug.Log( "[FinishGameAsync] Record Added" );
		
			Game.UI.FieldComplete.Open( Game.Mode.Board, Game.Mode.RunTime );
			
			Debug.Log( "[FinishGameAsync] Sequence of windows started" );
			
			await Handle.WaitBack( );
		
			Debug.Log( "[FinishGameAsync] returned back to hud so close stage" );
		
			// This will close through CoregameLoader
			GameStage.CloseStage( );
		}
    }
}