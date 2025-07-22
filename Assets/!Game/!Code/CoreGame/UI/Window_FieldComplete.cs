namespace Flexy.Template.BarleyBreak.CoreGame.UI
{
	public class Window_FieldComplete : UIWindowEx
    {
        [Bindable]		Single		Seconds				=> (((EField,Single))OpenParams).Item2;
        [Bindable]		String		FormattedSeconds	=> Seconds >= 60 ? TimeSpan.FromSeconds( Seconds ).ToString( @"mm\:ss\.ff" ) : TimeSpan.FromSeconds( Seconds ).ToString( @"ss\.ff" );

        protected override	Boolean	TryGoBack	( )		=> false;

        [Callable]		void		Continue	( )	
		{
			Game.UI.Leaderboards.Open( (((EField,Single))OpenParams).Item1 );
			Close( );
		}

		public record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	void		Open	( EField field, Single score ) => Ctx.Open( (field, score) );
		}
		
		[StateTest]		Object	Scoew_98	( ) => (EField.Board3x3, 98.1f);
		[StateTest]		Object	Scoew_23	( ) => (EField.Board3x3, 23.5f);
		[StateTest]		Object	Scoew_03	( ) => (EField.Board3x3, 0.52f); 
    }
}