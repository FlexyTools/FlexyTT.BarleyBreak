namespace FlexyTemplates.BarleyBreak.Coregame
{
	public class State_Pause : StateEx
	{
		protected override	void	OnShow	( )		=> Time.timeScale	= 0.00001f;
		protected override	void	OnHide	( )		=> Time.timeScale	= 1f;

		[Callable] void		Resume			( )		=> Close();
		[Callable] void		OpenSettings	( )		=> Game.States.GameSettings.Open();
		[Callable] void		LeaveField		( )		=> Game.Flow.ExitCoregame();
	}
}