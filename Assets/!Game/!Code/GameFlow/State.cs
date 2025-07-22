namespace Flexy.Template.BarleyBreak.GameFlow
{
	public abstract class	FlowItem: APropertyBindableBehaviour
	{
	
	}

	public abstract class	State: FlowItem
	{
		public object OpenParams;
	
		protected virtual	Boolean	TryGoBack	( )	=> false;
		protected virtual	void	OnShow		( )	{}
		protected virtual	void	OnHide		( )	{}
		protected virtual 	void 	OnFwdHide	( ) {}
		protected virtual 	void 	OnBackShow	( ) {}

		public void Close()
		{
			throw new NotImplementedException();
		}
		
		public record struct Opener( OpenCtx Ctx )
		{
			public	void		Open	( )					=> Ctx.Open( null );
		}
	}
	
	public record struct OpenCtx(State Ctx)
	{
		public void Open(Object openParams)
		{
			throw new NotImplementedException();
		}
	}
	
	
	public interface IOpener{}
	
	public class StateTestAttribute: Attribute{}
}