namespace Flexy.Template.BarleyBreak.Coregame.States
{
	// Visually this state is Coregame HUD
    public class State_Play : StateEx
    {
		[Bindable]	String	RunMinutes		=> TimeSpan.FromSeconds( Game.Mode.RunTime ).ToString( @"mm" );
		[Bindable]	String	RunSeconds		=> TimeSpan.FromSeconds( Game.Mode.RunTime ).ToString( @"ss" );
		[Bindable]	String	RunMiliseconds	=> TimeSpan.FromSeconds( Game.Mode.RunTime ).ToString( @"ff" );

		private Boolean _finishingStarted;

		protected override void		OnShow		( )		
		{
			_finishingStarted = false;
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
			
			RebindAll( );
		}
		private		void	OnApplicationPause	( Boolean pauseStatus )	
		{
			if( !Application.isEditor )
				Game.States.Pause.Open( );
		}
		
        [Callable]	void	Pause				( )			
        {
			Game.States.Pause.Open( );
        }
		
		private async	UniTaskVoid		FinishGameAsync	( )	
		{
			await UniTask.Delay( 1500, DelayType.UnscaledDeltaTime );
		
			Game.Leaderboards.AddRecord( Game.Mode.Board, Game.Mode.RunTime );

			Debug.Log( "[FinishGameAsync] Record Added" );
		
			Game.States.FieldComplete.Open( Game.Mode.Board, Game.Mode.RunTime );
			
			Debug.Log( "[FinishGameAsync] Sequence of windows started" );
			
			await UniTask.WaitWhile( () => gameObject.activeInHierarchy );
			await UniTask.WaitWhile( () => !gameObject.activeInHierarchy, PlayerLoopTiming.PreLateUpdate );
		
			Debug.Log( "[FinishGameAsync] returned back to hud so close stage" );
		
			// This will close through CoregameLoader
			GameStage.Close();
			Node.Graph.TransitionNow();
		}
    }
}