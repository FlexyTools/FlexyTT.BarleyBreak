namespace Flexy.Template.BarleyBreak.CoreGame.UI
{
	public class Window_Pause : UIWindowEx
	{
		protected override	void	OnShow	( )		=> Time.timeScale	= 0.00001f;
		protected override	void	OnHide	( )		=> Time.timeScale	= 1f;

		[Callable] void		Resume			( )		=> Close();
		[Callable] void		OpenSettings	( )		=> Game.UI.GameSettings.Open();
		[Callable] void		ExitBattle		( )		=> GameStage.CloseAllStates();
	}
}