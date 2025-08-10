using Flexy.Core.Binding;

namespace Flexy.Template.BarleyBreak.Boot
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Boot : GameStageEx
	{
		[SerializeField]	GameObject _loaderOverlay;
		[SerializeField]	AssetRef<State>[] _bootStates;
		[SerializeField]	AssetRef<GameStage> _meta;
	
		[Bindable] Int32	LoadingProgress		=> (Int32)(LoadingProgress01 * 100);
        [Bindable] Single	LoadingProgress01	=> _loadTask.Progress; 
        
		private LoadSceneTask	_loadTask;

		protected override	void	OnShow					( )		
		{
			BootGame( ).Forget( );
		}
		protected override	void	OnFirstChildShow		( )		
		{
			_loadTask = default;
		}
		
		private async	UniTask		BootGame		( )		
		{
			_loaderOverlay.gameObject.SetActive(true);
		
			await UniTask.Delay( 1_000, ignoreTimeScale:true );
			await Context.WaitInitializing();

			_loaderOverlay.gameObject.SetActive(false);

			foreach (var state in _bootStates)
			{
				var h = Graph.Open(state, this);
				
				while(h.IsOpened)
					await UniTask.Yield(PlayerLoopTiming.LastUpdate);
			}
			
			Graph.Open( _meta, Context );
		}
	}
}