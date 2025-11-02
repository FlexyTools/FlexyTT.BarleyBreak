using UnityEngine.SceneManagement;

namespace FlexyTemplates.BarleyBreak.Coregame
{
	[ServiceTypes(typeof(GameStage))]
	public class BarleyBreak_Coregame : GameStageEx, IStateWithResult<(EField, Single)>
	{
		[SerializeField]	GameObject _loaderOverlay = null!;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	=> _loadTask.Progress; 
        
		private LoadSceneTask	_loadTask;
		private EField			_resultBoard;
		private Single			_resultScore;
		private Boolean			_isLeaving;
		
		public	void				LeaveField				( )		
		{
			_isLeaving = true;
			CloseSubStates(true);
		}
		public	(EField,Single)		GetResult				( )		=> (_resultBoard,_resultScore);

		protected override	void	OnShow					( )		
		{
			Game.Audio.SwitchToCore();
		
			if (AnySubStateOpened)
			{
				_loaderOverlay.SetActive(false);
				return;
			}
			
			LoadGameFieldScene().Forget();
		}
		protected override	void	OnFirstChildShow		( )		
		{
			_loadTask = default;
		}
		protected override	void	OnLastChildHide			( )		
		{
			if (_isLeaving)
			{
				_resultBoard = default;
				_resultScore = default;
			}
			else
			{
				_resultBoard = Game.Mode.Board;
				_resultScore = Game.Mode.Result;
			}
			UnloadGameFieldScene().Forget();
		}
		protected override	void	OnHide					( )		
		{
			Game.Audio.SwitchToMeta();
			_loadTask = default;
		}
		
		private async	UniTask		LoadGameFieldScene		( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			Scene loadedScene;
			
			if ( OpenParams == null )
			{
				//We started from coregame scene so just simulate short loading and open root state
				loadedScene		= SceneManager.GetActiveScene();
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
			Game.RecacheCtx();
			
			_loaderOverlay.gameObject.SetActive(false);
		}
		private async	UniTask		UnloadGameFieldScene	( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			GameStage.MoveToServiceScene();
			await UniTask.NextFrame();
			
			_loadTask = SceneRef.LoadUrpDummySceneAsync( gameObject, LoadSceneMode.Single );
			
			await _loadTask;
			await UniTask.Delay( 350, ignoreTimeScale:true );
			
			CloseAndDestroy();
		}
	}
}