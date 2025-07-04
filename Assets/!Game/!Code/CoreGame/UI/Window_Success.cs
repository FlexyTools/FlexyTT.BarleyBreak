namespace Flexy.Template.BarleyBreak.CoreGame.UI
{
	public class Window_Success : UIWindowEx
    {
        [Bindable]		Single		Seconds				=> (Single)OpenParams;
        [Bindable]		String		FormattedSeconds	=> TimeSpan.FromSeconds( Seconds ).ToString( "g" );

        protected override	Boolean	TryGoBack	( )		=> false;

        [Callable]		void		Continue	( )	
		{
			GameStage.CloseStage( );
		}
        
		public record struct Opener( OpenCtx Ctx ) : IOpener
		{
			public	StateHandle		Open	( Single score ) => Ctx.Open( score );
		}
    }
}