namespace Flexy.Template.BarleyBreak;

public abstract class	UIWindowEx: State					
{
	private Facade_Game		_game; 
	public	Facade_Game		Game		=> _game.GetCached( this );
}
	
public abstract class	UIPopupEx: UIWindowEx { }
	
public class			UIWidgetEx:	APropertyBindableBehaviour	
{
	private	State			_panel;
	private Facade_Game		_game;
		
	public	Facade_Game		Game		=> _game.GetCached( this );
	public	State			Panel		=> _panel == null ? _panel = gameObject.GetComponentInParent<State>( true ) : _panel; 
}