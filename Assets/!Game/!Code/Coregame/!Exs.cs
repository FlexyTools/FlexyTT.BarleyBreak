namespace FlexyTemplates.BarleyBreak.Coregame;

public abstract class	GameStageEx: GameStage					
{
	private Facade_Coregame	_game; 
	public	Facade_Coregame	Game		=> _game.GetCached( this );
}

public abstract class	StateEx: State					
{
	private Facade_Coregame	_game; 
	public	Facade_Coregame	Game		=> _game.GetCached( this );
}

public abstract class	MonoBehEx: MonoBehaviour				
{
    public	Facade_Coregame	_game; 
    public	Facade_Coregame	Game		=> _game.GetCached( this ); 
}