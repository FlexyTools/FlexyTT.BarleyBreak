namespace Flexy.Template.BarleyBreak.CoreGame.UI
{
	public class Window_FieldComplete : UIWindowEx
    {
        [Bindable]		Single		Seconds				=> (((EField,Single))OpenParams).Item2;
        [Bindable]		String		FormattedSeconds	=> TimeSpan.FromSeconds( Seconds ).ToString( "g" );

        protected override	Boolean	TryGoBack	( )		=> false;

        [Callable]		void		Continue	( )	
		{
			Game.UI.Leaderboards.Open( (((EField,Single))OpenParams).Item1 );
			Close( );
		}
        
		public record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	StateHandle		Open	( EField field, Single score ) => Ctx.Open( (field, score) );
		}
		
		[StateTest]		Object	Scoew_54	( ) => (EField.Board3x3, 54.1f);
		[StateTest]		Object	Scoew_23	( ) => (EField.Board3x3, 23.5f); 
    }
}