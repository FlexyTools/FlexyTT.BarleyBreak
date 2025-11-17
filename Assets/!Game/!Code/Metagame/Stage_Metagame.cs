namespace FlexyTemplates.BarleyBreak.Metagame
{
	[ServiceTypes(typeof(GameStage))]
	public class Stage_Metagame : GameStageEx
	{
		private const	String			CoreGameStage = "c1055f23b34e09a4496fc2c881bb0920";

		private			Facade_Game		_game; 
		private			Facade_Game		Game		=> _game.GetCached( this );

		public async	UniTaskVoid		Play_Field	( SceneRef map )	
		{
			var playStageHandle	= new ResultHandle<(EField, Single)>( Graph.Open( new(CoreGameStage), Context.Parent, map ));
			var (field, score)	= await playStageHandle.WaitResult();
		
			if (field == default & score == default) // If data is empty then field is not completed
				return;
		
			Game.Leaderboards.AddRecord( field, score );
			Game.UI.Leaderboards.Open( field );
		}
	}
}