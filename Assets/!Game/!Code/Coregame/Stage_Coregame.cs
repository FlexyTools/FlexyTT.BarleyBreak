using FlexyTemplates.BarleyBreak.Coregame.Kit;
using UnityEngine.SceneManagement;

namespace FlexyTemplates.BarleyBreak.Coregame
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Coregame : GameStageEx, IStateWithResult<(EField, Single)>
	{
		[SerializeField]	GameObject _loaderOverlay = null!;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	{ get; set; } 
        
		private EField			_resultBoard;
		private Single			_resultScore;
		private Boolean			_isLeaving;
		private GameMode?		_gameMode;

		public	void				Exit				( )		
		{
			_isLeaving = true;
			CloseSubStates(true);
		}
		public	(EField,Single)		GetResult			( )		=> (_resultBoard,_resultScore);

		protected override	void	OnShow				( )		
		{
			enabled = false;
			Game.Audio.SwitchToCore();
		
			if (AnySubStateOpened) // Case of special boot from state scene
			{
				_loaderOverlay.SetActive(false);
				return;
			}
			
			LoadMap().Forget();
		}
		protected override	void	OnLastChildHide		( )		
		{
			_resultBoard = default;
			_resultScore = default;
		
			if (!_isLeaving && _gameMode != null)
			{
				_resultBoard = _gameMode.Board;
				_resultScore = _gameMode.Result;
			}
			
			UnloadMap().Forget();
		}
		protected override	void	OnHide				( )		
		{
			Game.Audio.SwitchToMeta();
		}
		
		public			void		StartPlay			( )		
		{
			Game.RecacheCtx();
			_gameMode = Game.Ctx.GetService<GameMode>();
		
			if (_gameMode == null)
				return;
		
			CloseSubStates(true);
			Game.States.Play.Open();
			
			enabled = true;
			_gameMode.StartPlay();
		}
		private			void		Update				( )		
		{
			if (_gameMode == null || !_gameMode.IsWin)
				return;
			
			enabled = false;
			FinishGameAsync().Forget();
				
			async UniTaskVoid FinishGameAsync ( ) 
			{
				await UniTask.Delay( 1000, DelayType.UnscaledDeltaTime );
				CloseSubStates(true);
				Game.States.PlayComplete.Open( _gameMode.Board, _gameMode.Result );
			}
		}
		
		private async	UniTask		LoadMap				( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			Scene loadedScene;
			
			if (OpenParams == null)
			{
				//We started from coregame scene so just simulate short loading and open root state
				await UniTask.Delay( 100, ignoreTimeScale:true );
				loadedScene			= SceneManager.GetActiveScene();
				LoadingProgress01	= 1.0f;
			}
			else
			{
				LoadingProgress01	= 0.0f;
				var sceneRef		= (SceneRef)OpenParams;
				var loadTask		= sceneRef.LoadSceneAsync( gameObject, LoadSceneMode.Single );
				
				while (!loadTask.IsDone)
				{
					LoadingProgress01	= loadTask.Progress * 0.9f;
					await UniTask.NextFrame();
				}
				
				loadedScene	= loadTask.Scene;
				await GameContext.GetCtx(loadedScene).WaitInitialization();
				
				LoadingProgress01	= 1.0f;
			}
			
			await UniTask.Delay( 350, ignoreTimeScale:true );
			GameStage.MoveToLoadedScene( loadedScene );
			
			_gameMode = loadedScene.GetService<GameMode>();
			
			GameStage.OpenMainState();
			Game.RecacheCtx();
			
			_loaderOverlay.gameObject.SetActive(false);
		}
		private async	UniTask		UnloadMap			( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
			GameStage.MoveToServiceScene();
			
			await UniTask.NextFrame();
			await SceneRef.LoadUrpDummySceneAsync( gameObject, LoadSceneMode.Single );
			await UniTask.Delay( 350, ignoreTimeScale:true );
			
			CloseAndDestroy();
		}
	}
}