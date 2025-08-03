namespace Flexy.Template.BarleyBreak.CoreGame;

public abstract class	GameStageEx: GameStage					
{
	private Facade_Coregame	_game; 
	public	Facade_Coregame	Game		=> _game.GetCached( this );
}

public abstract class	GameStateEx: State					
{
	private Facade_Coregame	_game; 
	public	Facade_Coregame	Game		=> _game.GetCached( this );
}

public class			MonoBehEx: MonoBehaviour				
{
    public	Facade_Coregame	_game; 
    public	Facade_Coregame	Game		=> _game.GetCached( this ); 
}
	
public abstract class	UIWindowEx: State					
{
    private Facade_Coregame	_game; 
    public	Facade_Coregame	Game		=> _game.GetCached( this );
}