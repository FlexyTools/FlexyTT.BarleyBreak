namespace Flexy.Template.BarleyBreak.Boot
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Boot : GameStageEx
	{
		[SerializeField]	GameObject			_loaderOverlay;
		[SerializeField]	AssetRef<State>		_boot_eula;
		[SerializeField]	AssetRef<GameStage>	_metaStageRef;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	=> _loadTask.Progress; 
        
        private BooleanSetting	_eulaAccepted;
		private LoadSceneTask	_loadTask;

		protected override	void	OnShow				( )		
		{
			_eulaAccepted	= new("Boot_EulaAccepted", false);
			BootGame().Forget();
		}
		private async	UniTask		BootGame			( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			await UniTask.Delay( 1_000, ignoreTimeScale:true );
			await Context.WaitInitializing();

			_loaderOverlay.gameObject.SetActive(false);

			if (!_eulaAccepted)
			{
				await ShowState(_boot_eula);
				_eulaAccepted.Set(true);
			}
			
			// await ShowState(_boot_age);
			// await ShowState(_boot_intro);

			async UniTask ShowState( AssetRef<State> state )
			{
				var h = Graph.Open( state, this );
				
				while (h.IsOpened)
					await UniTask.Yield( PlayerLoopTiming.LastUpdate );
			}
			
			Graph.Open( _metaStageRef, Context );
		}
	}
}