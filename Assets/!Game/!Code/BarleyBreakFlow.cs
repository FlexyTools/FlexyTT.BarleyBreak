namespace FlexyTemplates.BarleyBreak
{
	public class BarleyBreakFlow : MonoBehaviour, IService
	{
		private const	String			CoreGameStage = "c1055f23b34e09a4496fc2c881bb0920";

		private			Facade_Game		_game; 
		private			Facade_Game		Game		=> _game.GetCached( this );

		public async	UniTaskVoid		Play_Field	( SceneRef map )	
		{
			var playStageHandle	= new ResultHandle<(EField, Single)>( Game.Ctx.GetService<Service_GameFlow>().Graph.Open( new(CoreGameStage), Game.Ctx.Parent, map ));
			var (field, score)	= await playStageHandle.WaitResult();
		
			Game.Leaderboards.AddRecord( field, score );
			Game.UI.Leaderboards.Open( field );
		}

		public			void			OrderedInit	( GameContext ctx )	
		{
			// We can make super first game init here,
			// but currently it is used only to make sure GameContext will add this to context
		}
	}
}