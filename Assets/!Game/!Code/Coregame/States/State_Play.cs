namespace FlexyTemplates.BarleyBreak.Coregame.States
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
				FinishGameAsync().Forget();
				enabled = false;
			}	
			
			RebindAll();
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
			Game.Leaderboards.AddRecord( Game.Mode.Board, Game.Mode.RunTime );
			
			await UniTask.Delay( 1000, DelayType.UnscaledDeltaTime );

			GameStage.CloseSubStates(true);
			
			Game.States.FieldComplete.Open( Game.Mode.Board, Game.Mode.RunTime );
		}
    }
}