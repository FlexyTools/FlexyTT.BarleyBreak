namespace FlexyTemplates.BarleyBreak.Coregame;

public abstract class	GameStageEx: GameStage					
{
	private		Facade_Coregame	_game; 
	public	ref Facade_Coregame	Game	=> ref _game.GetCached( this );
}

public abstract class	StateEx: State					
{
	private		Facade_Coregame	_game; 
	public	ref Facade_Coregame	Game	=> ref _game.GetCached( this );
}

public abstract class	MonoBehEx: MonoBehaviour				
{
    public		Facade_Coregame	_game; 
    public	ref Facade_Coregame	Game	=> ref _game.GetCached( this ); 
}