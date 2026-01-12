using FlexyTT.BarleyBreak.Coregame.Kit;

namespace FlexyTT.BarleyBreak.Coregame
{
	// Visually this state is Coregame HUD
    public class State_Play : StateEx
    {
		[Bindable]	String	RunMinutes		=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"mm" );
		[Bindable]	String	RunSeconds		=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"ss" );
		[Bindable]	String	RunMilliseconds	=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"ff" );

		private GameMode _gameMode = null!;

		protected override UniTask	OnShow		( )		
		{
			if (OpenParams is SceneRef sceneRef)
			{
				// We started from test scene because runtime flow dont have Params at all for this State
				// So close to GameStage with replay request to load requested map
				GameStage.CloseSubStates(true, false, (true, sceneRef));
				return default;
			}
		
			_gameMode = gameObject.GetService<GameMode>();
			return default;
		}
		protected override Boolean	TryGoBack	( )		
		{
			Pause();
			return false;
		}

		private		void	Update				( )						
		{
			RebindAll();
		}
		private		void	OnApplicationPause	( Boolean pauseStatus )	
		{
			if (!Application.isEditor)
				Game.States.Pause.Open();
		}
		
        [Callable]	void	Pause				( )			
        {
			Game.States.Pause.Open();
        }
        
        [StateTest]	Object	Play_3x3 	( ) => new SceneRef("1afdd18b2b9c65e4b866284337ba9044");
        [StateTest]	Object	Play_4x4 	( ) => new SceneRef("6bfc812ba38db7b47bc55722377eb3a5");
        [StateTest]	Object	Play_5x5 	( ) => new SceneRef("3fb4186ae0d813b44b8097bf7a82b451");
    }
}