namespace FlexyTemplates.BarleyBreak.Coregame;

public abstract class	GameStageEx: GameStage					
{
	private		Facade_Coregame	_game; 
	public	ref Facade_Coregame	Game	=> ref _game.GetCached( this );
}

public abstract class	StateEx: State					
{
	private		Facade_Coregame	_game; 
	public	ref Facade_Coregame	Game		=> ref _game.GetCached( this );
	public new	Stage_Coregame	GameStage	=> (Stage_Coregame)Node.GameStageNode.State;
}

public abstract class	MonoBehEx: MonoBehaviour				
{
    public		Facade_Coregame	_game; 
    public	ref Facade_Coregame	Game	=> ref _game.GetCached( this ); 
}

public abstract class	UIWindowEx: UIWindow					
{
	private		Facade_Coregame	_game; 
	public	ref Facade_Coregame	Game		=> ref _game.GetCached( this );
}
	
public abstract class	UIWidgetEx:	BindableBehaviour	
{
	private	State?				_state;
	private Facade_Coregame		_game;
		
	public	ref Facade_Coregame	Game		=> ref _game.GetCached( this );
	public		State			State		=> _state == null ? _state = gameObject.GetComponentInParent<State>( true ) : _state; 
}