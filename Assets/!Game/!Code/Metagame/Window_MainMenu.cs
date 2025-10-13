namespace FlexyTemplates.BarleyBreak.Metagame
{
	public class Window_MainMenu : UIWindowEx
	{
		[Callable]	void	OpenPlayFields		( )		
		{
			Game.UI.PlayFields.Open( );
		}
		[Callable]	void	OpenSettings		( )		
		{
			Game.UI.Settings.Open( );
		}
		[Callable]	void	ExitGame			( )		
		{
			if( !Application.isEditor )
			{
				Application.Quit( );
			}
			else
			{
	#if UNITY_EDITOR
				UnityEditor.EditorApplication.ExitPlaymode( );
	#endif
			}
		}
	}
}