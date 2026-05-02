namespace FlexyTT.BarleyBreak.Menu
{
	public class Popup_ExitGame : UIPopupEx
	{
		[Callable]	void	ExitGame			( )		
		{
			if (!Application.isEditor)
			{
				Application.Quit();
			}
			else
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.ExitPlaymode();
#endif
			}
		}	
	}
}