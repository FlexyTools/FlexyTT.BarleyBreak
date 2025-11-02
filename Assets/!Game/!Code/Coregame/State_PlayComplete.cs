namespace FlexyTemplates.BarleyBreak.Coregame
{
	public class State_PlayComplete : StateEx
    {
	    private	(EField Field, Single Seconds) Params => ((EField,Single))OpenParams!;
    
        [Bindable]		Single		Seconds				=> Params.Seconds;
        [Bindable]		String		FormattedSeconds	=> Seconds >= 60 ? TimeSpan.FromSeconds( Seconds ).ToString( @"mm\:ss\.ff" ) : TimeSpan.FromSeconds( Seconds ).ToString( @"ss\.ff" );

        protected override	Boolean	TryGoBack	( )		=> false;

        [Callable]			void	Continue	( )			
		{
			Close();			
		}
        
		public new record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	StateHandle		Open	( EField field, Single seconds ) => Ctx.Open( (field, seconds) );
		}
		
		[StateTest]		Object	Scoew_98	( ) => (EField.Board3x3, 98.1f);
		[StateTest]		Object	Scoew_23	( ) => (EField.Board3x3, 23.5f);
		[StateTest]		Object	Scoew_052	( ) => (EField.Board3x3, 0.52f); 
    }
}