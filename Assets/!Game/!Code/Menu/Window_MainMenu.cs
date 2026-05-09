namespace FlexyTT.BarleyBreak.Menu
{
	public class Window_MainMenu : UIWindowEx
	{
		protected override	Boolean	TryGoBack	( )	=> false;

		[Callable]	void	OpenPlayFields		( ) => Game.UI.PlayFields	.Open();
		[Callable]	void	OpenSettings		( ) => Game.UI.Settings		.Open();
		[Callable]	void	ExitGame			( ) => Game.UI.Popup_Exit	.Open();
		[Callable]	void	OpenAppInfo			( ) => Game.UI.AppInfo		.Open();
	}
}