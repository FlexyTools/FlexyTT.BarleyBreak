namespace Flexy.Template.BarleyBreak
{
	public class			MonoBehEx: MonoBehaviour				
	{
		public	Facade_Game		_game; 
		public	Facade_Game		Game => _game.GetCached( this ); 
	}
	
	public abstract class	UIWindowEx: UIWindow					
	{
		private Facade_Game		_game; 
		public	Facade_Game		Game		=> _game.GetCached( this );

	}
	public abstract class	UITabsWindowEx: UITabsWindow					
	{
		private Facade_Game		_game; 
		public	Facade_Game		Game		=> _game.GetCached( this );
	}
	
	public abstract class	UIPopupEx: UIWindowEx					
	{
		
	}
	
	public abstract class	UIPopupExWithResult<T>: UIPopupEx, IStateWithResult<T>					
	{
		protected	T	_result;
		public		T	GetResult() { return _result; }
		protected override void OnShow() { _result = default; }
	}
	
	public class			WidgetEx:	APropertyBindableBehaviour	
	{
		private	FlowItem		_panel;
		private Facade_Game		_game;
		
		public	Facade_Game		Game		=> _game.GetCached( this );
		public	FlowItem		Panel		=> _panel == null ? _panel = gameObject.GetComponentInParent<FlowItem>( true ) : _panel; 
	}
}