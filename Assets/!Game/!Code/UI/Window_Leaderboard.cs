namespace Flexy.Template.BarleyBreak.UI
{
	public class Window_Leaderboard : UIWindowEx
	{
		[Bindable]	Boolean		ShowSingle	=> OpenParams is EField;
		[Bindable]	EField		ShowBoard	=> (EField)OpenParams;
		
		[Bindable]	Collection	Records3x3 => new (Game.Leaderboards.Leaderboard3X3.Records, Setup);
		[Bindable]	Collection	Records4x4 => new (Game.Leaderboards.Leaderboard4X4.Records, Setup);
		[Bindable]	Collection	Records5x5 => new (Game.Leaderboards.Leaderboard5X5.Records, Setup);
		[Bindable]	Collection	Records => (EField)OpenParams switch   
		{ 
			EField.Board3x3 => Records3x3,
			EField.Board4x4 => Records4x4,
			EField.Board5x5 => Records5x5
		};

		private void Setup( GameObject widget, Object data, Boolean isNew, Int32 index )
		{
			var seconds	= (Single)data;
			var value	= Single.IsPositiveInfinity(seconds) ? "-" : seconds >= 60 
				? TimeSpan.FromSeconds( seconds ).ToString( @"mm\:ss\.ff" ) 
				: TimeSpan.FromSeconds( seconds ).ToString( @"ss\.ff" );
				
			widget.GetComponent<BindableDataStore>( ).SetValue( "Score", value );
		}
		
		public record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	StateHandle		Open	( )					=> Ctx.Open( null );
			public	StateHandle		Open	( EField board )	=> Ctx.Open( board );
		}
		
		[StateTest]	Object	Board3x3	( ) => EField.Board3x3;
		[StateTest]	Object	Board4x4	( ) => EField.Board4x4;
		[StateTest]	Object	Board5x5	( ) => EField.Board5x5;
	}
}