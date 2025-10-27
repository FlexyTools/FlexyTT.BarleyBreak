using System.Linq;

namespace FlexyTemplates.BarleyBreak.Metagame.Leaderboards
{
	public class Window_Leaderboards : UIWindowEx
	{
		[Bindable]	Collection	Records3x3 	=> new (Game.Leaderboards.Board3X3.Records.Select(s => new ScoreView(s)));
		[Bindable]	Collection	Records4x4 	=> new (Game.Leaderboards.Board4X4.Records.Select(s => new ScoreView(s)));
		[Bindable]	Collection	Records5x5 	=> new (Game.Leaderboards.Board5X5.Records.Select(s => new ScoreView(s)));
		
		[Bindable]	Boolean		ExitToRight	=> OpenParams is not null;
		[Bindable]	Boolean		IsShowBoard	( EField field )	=> OpenParams is not EField f || f == field;
		
		[StateTest]	Object		Board3x3	( ) => EField.Board3x3;
		[StateTest]	Object		Board4x4	( ) => EField.Board4x4;
		[StateTest]	Object		Board5x5	( ) => EField.Board5x5;
		
		private record ScoreView(Single Score)
		{
			[Bindable]	Single	Score		{get;init;} = Score;
			[Bindable]	String	ScoreStr	=> Single.IsPositiveInfinity(Score) ? "-" : TimeSpan.FromSeconds( Score ).ToString( Score >= 60 ? @"mm\:ss\.ff" : @"ss\.ff" );
		}
		
		public record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	StateHandle		Open	( )					=> Ctx.Open( null );
			public	StateHandle		Open	( EField board )	=> Ctx.Open( board );
		}
	}
}