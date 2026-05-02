namespace FlexyTT.BarleyBreak.Play
{
	public class State_Pause : StateEx
	{
		protected override	UniTask	OnShow	( )		
		{
			Node.TransitionHost.LockOn(Node);
			Time.timeScale	= 0.00001f;	
			return default; 
		}
		protected override	UniTask	OnHide	( )		
		{
			Time.timeScale	= 1f;		
			return default; 
		}

		[Callable] void		Resume			( )		=> Close();
		[Callable] void		OpenSettings	( )		=> Game.States.GameSettings.Open();
		[Callable] void		LeaveField		( )		=> Game.Flow.ExitCoregame();
		
		[Callable] void		Share			( )		=> Game.Global.NoInternet.Open();
	}
}