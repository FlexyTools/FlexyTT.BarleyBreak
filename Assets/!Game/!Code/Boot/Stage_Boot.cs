namespace FlexyTT.BarleyBreak.Boot
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Boot : GameStageEx
	{
		[SerializeField]	GameObject			_loaderOverlay = null!;
		[SerializeField]	AssetRef<State>[]	_bootStates = null!;
		[SerializeField]	AssetRef<GameStage>	_metaStageRef;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	=> _loadTask?.Progress ?? 1; 
        
		private LoadSceneTask?	_loadTask;

		protected override	UniTask			OnShow		( )		
		{
			BootGame().Forget();
			return default;
		}
		private async		UniTask			BootGame	( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			await UniTask.Delay(1_000, ignoreTimeScale:true);
			await Context.WaitInitialization();

			_loaderOverlay.gameObject.SetActive(false);

			for (var i = await GetInitialStateIndex(); i < _bootStates.Length; i++)
				await Graph.Open(_bootStates[i], Node).WaitClose();
			
			Graph.Open(_metaStageRef, Context);
		}
		
		private async		UniTask<Int32>	GetInitialStateIndex ( )	
		{
			// Check if we have test boot state already opened
			if (!AnySubStateOpened) 
				return 0;
				
			var booti = 1 + Array.IndexOf(_bootStates, Node.FirstBaseChild?.State.PrefabRef);

			var h = Node.FirstBaseChild!;
			while (h.IsOpened)
				await UniTask.Yield(PlayerLoopTiming.LastUpdate);

			return booti;
		}
	}
}