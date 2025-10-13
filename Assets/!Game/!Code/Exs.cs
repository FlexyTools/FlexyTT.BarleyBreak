namespace FlexyTemplates.BarleyBreak;

public abstract class	UIWindowEx: UIWindow					
{
	private Facade_Game		_game; 
	public	Facade_Game		Game		=> _game.GetCached( this );
}
	
public abstract class	UIWidgetEx:	BindableBehaviour	
{
	private	State			_state;
	private Facade_Game		_game;
		
	public	Facade_Game		Game		=> _game.GetCached( this );
	public	State			State		=> _state == null ? _state = gameObject.GetComponentInParent<State>( true ) : _state; 
}