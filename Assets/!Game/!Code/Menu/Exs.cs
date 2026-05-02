namespace FlexyTT.BarleyBreak.Menu;

public abstract class	GameStageEx: GameStage					
{
	private		Facade_Game	_game; 
	public	ref Facade_Game	Game	=> ref _game.GetCached( this );
}

public abstract class	StateEx: State					
{
	private		Facade_Game	_game; 
	public	ref Facade_Game	Game	=> ref _game.GetCached( this );
}

public abstract class	MonoBehEx: MonoBehaviour				
{
	public		Facade_Game	_game; 
	public	ref Facade_Game	Game	=> ref _game.GetCached( this ); 
}

public abstract class	UIWindowEx: UIWindow					
{
	private		Facade_Game	_game; 
	public	ref Facade_Game	Game	=> ref _game.GetCached( this );
}

public abstract class	UIPopupEx: UIWindow					
{
	private		Facade_Game	_game; 
	public	ref Facade_Game	Game	=> ref _game.GetCached( this );
}
	
public abstract class	UIWidgetEx:	BindableBehaviour	
{
	private	State?			_state;
	private Facade_Game		_game;
		
	public	ref Facade_Game	Game	=> ref _game.GetCached( this );
	public		State		State	=> _state == null ? _state = gameObject.GetComponentInParent<State>( true ) : _state; 
}