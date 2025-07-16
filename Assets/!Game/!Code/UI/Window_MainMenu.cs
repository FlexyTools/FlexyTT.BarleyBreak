using Flexy.Core.Tweens;

namespace Flexy.Template.BarleyBreak.UI
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
		
		[SerializeField]	Transform _tr;

		private void Start()
		{
			Tween.Value(-675, -345, 1, Ease.InOutSine).BindTo( _tr, static (v, tr) => tr.localPosition = new Vector3(-400, v, 0)).Run();
		}
	}
}