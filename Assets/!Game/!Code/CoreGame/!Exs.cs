namespace Flexy.Template.BarleyBreak.CoreGame;

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