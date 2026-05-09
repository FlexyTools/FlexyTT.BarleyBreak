namespace FlexyTT.BarleyBreak.Menu
{
	[ServiceTypes(typeof(GameStage))]
	public class GameStage_Menu : GameStageEx
	{
		[SerializeField] FlowLibrary			_menuLibrary = null!;
		[SerializeField] AssetRef<GameStage>	_coreGameStage;
		
		private			Facade_Game		_game; 
		private			Facade_Game		Game		=> _game.GetCached( this );

		public async	UniTaskVoid		Play_Field	( SceneRef map )	
		{
			var (field, score) = await Graph.Open( _coreGameStage, map ).WaitResultOnHide<(EField, Single)>();
				
			if (field == default && score == default) // If data is empty then field is not completed
				return;
		
			Game.Leaderboards.AddRecord( field, score );
			Game.UI.Leaderboards.Open( field );
		}

		protected override UniTask		OnShow			( )		
		{
			this.PreloadLibrary(_menuLibrary, 1).Forget();
			
			return base.OnShow();
		}
		protected override UniTask		OnForwardHide	( )		
		{
			this.ClearStatesCache().Forget();
			
			return base.OnForwardHide();
		}
	}
}