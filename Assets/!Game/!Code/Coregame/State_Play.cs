using FlexyTemplates.BarleyBreak.Coregame.Kit;

namespace FlexyTemplates.BarleyBreak.Coregame
{
	// Visually this state is Coregame HUD
    public class State_Play : StateEx
    {
		[Bindable]	String	RunMinutes		=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"mm" );
		[Bindable]	String	RunSeconds		=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"ss" );
		[Bindable]	String	RunMilliseconds	=> TimeSpan.FromSeconds( _gameMode.RunTime ).ToString( @"ff" );

		private GameMode _gameMode = null!;

		protected override void		OnShow		( )		
		{
			_gameMode = gameObject.GetService<GameMode>();
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
    }
}