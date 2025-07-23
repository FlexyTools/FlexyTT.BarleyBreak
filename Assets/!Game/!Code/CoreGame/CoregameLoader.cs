using UnityEngine.SceneManagement;

namespace Flexy.Template.BarleyBreak.CoreGame
{
	public class CoregameLoader : UIWindowEx
	{
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	=> _loadTask.Progress; 
        
		private LoadSceneTask	_loadTask;

		protected override	void	OnShow				( )		
		{
			LoadField( ).Forget( );
		}
		protected override	void	OnFwdHide			( )		
		{
			_loadTask = default;
		}
		protected override	void	OnBackShow			( )		
		{
			UnloadField( ).Forget( );
		}
		protected override	void	OnHide				( )		
		{
			_loadTask = default;
		}
		
		private async	UniTask		LoadField			( )		
		{
			var loadedScene = default(Scene);
			
			if ( OpenParams == null )
			{
				//We started from coregame scene so just simulate short loading and open root state
				loadedScene		= SceneManager.GetActiveScene( );
			}
			else
			{
				var sceneRef	= (SceneRef)OpenParams;
				_loadTask		= sceneRef.LoadSceneAsync( gameObject, LoadSceneMode.Single );
				loadedScene		= await _loadTask;
			}
			
			await UniTask.Delay( 350 );
			GameStage.MoveToLoadedScene( loadedScene );
			GameStage.OpenRootState( );
		}
		private async	UniTask		UnloadField			( )		
		{
			GameStage.MoveToServiceScene( );
			
			_loadTask = SceneRef.LoadDummySceneAsync( gameObject, LoadSceneMode.Single );
			await _loadTask;
			await UniTask.Delay( 350 );
			
			Close( );
			GameStage.RemoveFromHistoryAfterClose( );
		}
	}
}