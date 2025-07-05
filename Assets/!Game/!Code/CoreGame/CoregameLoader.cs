using Flexy.Template.BarleyBreak.CoreGame.Minigames;
using UnityEngine.SceneManagement;

namespace Flexy.Template.BarleyBreak.CoreGame
{
	public class CoregameLoader : State, IStateWithResult<Single>
	{
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	{get;set;} 

        private Facade_Game	_game; 
        public	Facade_Game	Game		=> _game.GetCached( this );
        
        private SceneRef    _map;
        
		private Single		_runResult;

        protected override	void	OnShow				( )		
		{
			LoadField( ).Forget( Debug.LogException );
		}
		protected override	void	OnBackShow			( )		
		{
			_runResult = Game.Mode.RunTime;
			
			UnloadField( ).Forget( Debug.LogException );
		}

		private async	UniTask		LoadField			( )		
		{
			Debug.Log( $"[DTLoader_Coregame] ----------- ===========   Coregame Loading: START   =========== -----------" );
			
			var loadedScene = default(Scene);
			Time.timeScale	= 0.001f;
			
			if ( OpenParams == null ) //We started from coregame scene so just open hud
			{
				Debug.Log( $"[DTLoader_Coregame] ----------- ===========   LoadLocation:  current {SceneManager.GetActiveScene().name}   =========== -----------" );
				loadedScene			= SceneManager.GetActiveScene( );
				
				await UniTask.Delay( 100, DelayType.UnscaledDeltaTime );
				
				LoadingProgress01 = 1;
				RebindProperty( "LoadingProgress01" );
				
				await UniTask.Delay( 300, DelayType.UnscaledDeltaTime );
				
				// scene must be loaded beforehand so do nothing
			}
			else
			{
				var oldFramerate = Application.targetFrameRate;
				Application.targetFrameRate = 10;				//Do not need high framerate for loading screen
				
				var sceneRef	= (SceneRef)OpenParams;
				
				Debug.Log( $"[DTLoader_Coregame] ----------- ===========   LoadLocation: {AssetRef.AssetsLoader.GetSceneName( sceneRef )}   =========== -----------" );
				
				var task		= sceneRef.LoadSceneAsync( gameObject, LoadSceneMode.Single );
				
				await task.WaitForSceneLoadStart( );
				
				loadedScene = task.Scene;
				
				while( !task.IsDone )
				{
					LoadingProgress01 = task.Progress;
					RebindProperty( "LoadingProgress01" );
					await UniTask.NextFrame( PlayerLoopTiming.EarlyUpdate );
					
					//Make sure subscene is loaded, todo make normal explicit task for loading match and all content
					//var matchScene = await _loadTask.Task;
				}
				RebindProperty( "LoadingProgress01" );
				
				Application.targetFrameRate = oldFramerate;
			}
			
			Debug.Log( $"[DTLoader_Coregame] ----------- ===========   LoadLocation: DONE   =========== -----------" );
			
			SceneManager.MoveGameObjectToScene( GameStage.gameObject, loadedScene );
			GameStage.transform.SetSiblingIndex(0);
			
			await UniTask.NextFrame( );
			
			var      timeStartLoad = Time.realtimeSinceStartup;
			
			//Make sure loading screen is visible at least 1 second
			var elapsedTimeFromStartLoad	= Time.realtimeSinceStartup - timeStartLoad; 
			if( elapsedTimeFromStartLoad < 0.5f )
				await UniTask.Delay( TimeSpan.FromSeconds(1-elapsedTimeFromStartLoad), DelayType.Realtime );
			
			await UniTask.DelayFrame( 1 );
			Time.timeScale	= 1f;
			
			var loadedMinigame = FindAnyObjectByType<Minigame_BarleyBreak>( );
			GameStage.Context.SetService( loadedMinigame );
			
			GameStage.OpenRootState( );
			
			Debug.Log( $"[DTLoader_Coregame] ----------- ===========   Coregame Loading: DONE   =========== -----------" );
		}
		private async	UniTask		UnloadField			( )		
		{
			Debug.Log( $"[DTLoader_Coregame] ----------- ===========   Coregame Unloading: START   =========== -----------" );
			
			await UniTask.NextFrame( );

			GameStage.transform.parent = null;
			SceneManager.MoveGameObjectToScene( GameStage.gameObject, GameStage.FlowService.gameObject.scene );
			
			var timeStartLoad = Time.realtimeSinceStartup;
			var task = SceneRef.LoadDummySceneAsync( gameObject, LoadSceneMode.Single );
			
			while( !task.IsDone )
			{
				LoadingProgress01 = task.Progress;
				RebindProperty( "LoadingProgress01" );
				await UniTask.NextFrame( PlayerLoopTiming.EarlyUpdate );
			}
			
			RebindProperty( "LoadingProgress01" );
			
			Time.timeScale = 0.001f;
			await UniTask.DelayFrame( 2 );
			
			GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced, true, true );
			Resources.UnloadUnusedAssets( );
			GC.Collect( GC.MaxGeneration, GCCollectionMode.Forced, true, true );
			
			var elapsedTimeFromStartLoad = Time.realtimeSinceStartup - timeStartLoad; 
			
			if( elapsedTimeFromStartLoad < 1 )
				await UniTask.Delay( TimeSpan.FromSeconds(1-elapsedTimeFromStartLoad), DelayType.Realtime );
			
			GameStage.GetComponent<GameContext>( ).Destroy( );
			await UniTask.NextFrame( );
			
			Time.timeScale = 1;
			
			Close( );
			GameStage.RemoveFromHistoryAfterClose( );
			
			Debug.Log( $"[DTLoader_Coregame] ----------- ===========   Coregame Unloading: DONE   =========== -----------" );
		}
		
		public			Single		GetResult			( )	=> _runResult;
	}
}