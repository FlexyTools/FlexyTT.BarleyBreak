namespace Flexy.Template.BarleyBreak.CoreGame.UI
{
	public class Window_FieldComplete : UIWindowEx
    {
	    private	(EField Field, Single Seconds) Params => ((EField,Single))OpenParams;
    
        [Bindable]		Single		Seconds				=> Params.Seconds;
        [Bindable]		String		FormattedSeconds	=> Seconds >= 60 ? TimeSpan.FromSeconds( Seconds ).ToString( @"mm\:ss\.ff" ) : TimeSpan.FromSeconds( Seconds ).ToString( @"ss\.ff" );

        protected override	Boolean	TryGoBack	( )		=> false;

        [Callable]		void		Continue	( )	
		{
			GameStage.CloseAllStates();
			Game.UI.Leaderboards.Open( Params.Field );
		}
        
		public record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	StateHandle		Open	( EField field, Single seconds ) => Ctx.Open( (field, seconds) );
		}
		
		[StateTest]		Object	Scoew_98	( ) => (EField.Board3x3, 98.1f);
		[StateTest]		Object	Scoew_23	( ) => (EField.Board3x3, 23.5f);
		[StateTest]		Object	Scoew_03	( ) => (EField.Board3x3, 0.52f); 
    }
}