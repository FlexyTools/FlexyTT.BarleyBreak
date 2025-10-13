using UnityEngine.SceneManagement;

namespace FlexyTemplates.BarleyBreak.Coregame.States
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Coregame : GameStageEx
	{
		[SerializeField]	GameObject _loaderOverlay = null!;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	=> _loadTask.Progress; 
        
		private LoadSceneTask	_loadTask;

		protected override	void	OnShow					( )		
		{
			LoadGameFieldScene( ).Forget( );
		}
		protected override	void	OnFirstChildShow		( )		
		{
			_loadTask = default;
		}
		protected override	void	OnLastChildHide			( )		
		{
			UnloadGameFieldScene( ).Forget( );
		}
		protected override	void	OnHide					( )		
		{
			_loadTask = default;
		}
		
		private async	UniTask		LoadGameFieldScene		( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
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
			
			await UniTask.Delay( 350, ignoreTimeScale:true );
			GameStage.MoveToLoadedScene( loadedScene );
			GameStage.OpenMainState();
			
			_loaderOverlay.gameObject.SetActive(false);
		}
		private async	UniTask		UnloadGameFieldScene	( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			GameStage.MoveToServiceScene( );
			
			_loadTask = SceneRef.LoadDummySceneAsync( gameObject, LoadSceneMode.Single );
			await _loadTask;
			await UniTask.Delay( 350, ignoreTimeScale:true );
			
			CloseAndDestroy();
		}
	}
}