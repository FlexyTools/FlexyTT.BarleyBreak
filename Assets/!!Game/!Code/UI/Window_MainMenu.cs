namespace Runner.UI
{
	public class Window_MainMenu : UIWindowEx
	{
		[Callable]	void	OpenChooseZones		( )		
		{
			Game.UI.ChooseZone.Open( );
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