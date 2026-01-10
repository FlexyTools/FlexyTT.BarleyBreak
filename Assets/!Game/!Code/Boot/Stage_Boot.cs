namespace FlexyTemplates.BarleyBreak.Boot
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

		protected override	void	OnShow				( )		
		{
			BootGame().Forget();
		}
		private async	UniTask		BootGame			( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			await UniTask.Delay(1_000, ignoreTimeScale:true);
			await Context.WaitInitialization();

			_loaderOverlay.gameObject.SetActive(false);

			var booti = 0;

			// Check if we have test boot state already opened
			if (AnySubStateOpened)
			{
				booti = 1 + Array.IndexOf(_bootStates, Node.FirstChild?.State.PrefabRef);

				var h = Node.FirstChild!;
				while (h.IsOpened)
					await UniTask.Yield(PlayerLoopTiming.LastUpdate);
			}		

			for (; booti < _bootStates.Length; booti++)
				await ShowState(_bootStates[booti]);
			
			async UniTask ShowState( AssetRef<State> state )
			{
				var h = Graph.Open(state, Node);
				
				if (h.State is IBootState { IsDone: true } )
				{
					h.Close();
					return;
				}
				
				while (h.IsOpened)
					await UniTask.Yield(PlayerLoopTiming.LastUpdate);
			}
			
			Graph.Open(_metaStageRef, Context);
		}
	}

	internal interface IBootState
	{
		Boolean IsDone { get; }
	}
}