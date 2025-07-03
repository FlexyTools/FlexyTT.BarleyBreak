using Test.Runner.CoreGame.Minigames;
using Test.Runner.CoreGame.UI;

namespace Test.Runner.CoreGame;

public struct	Facade_Game
{
    public	GameContext			    Ctx				{ get; private set; }
													
    public	Facade_UIWindows_Core   UI				=> new(Ctx.GetService<GameStage>( ));
    public	GameSettingsService	    Settings		=> Ctx.GetService<GameSettingsService>( );
    public	AudioPlaylist		    AudioPlayer		=> Ctx.GetService<AudioPlaylist>( );
    public  Minigame_BarleyBreak	Mode         	=> Ctx.GetService<Minigame_BarleyBreak>( );

    public	Facade_Game			GetCached			( Component callSource )
    {
        if( !Ctx?.IsAlive ?? true )
            Ctx = GameContext.GetCtx(callSource);

        return this;
    }
}

public class			MonoBehEx: MonoBehaviour				
{
    public	Facade_Game	_game; 
    public	Facade_Game	Game		=> _game.GetCached( this ); 
}
	
public abstract class	UIWindowEx: UIWindow					
{
    private Facade_Game	_game; 
    public	Facade_Game	Game		=> _game.GetCached( this );

}
public abstract class	UITabsWindowEx: UITabsWindow					
{
    private Facade_Game	_game; 
    public	Facade_Game	Game		=> _game.GetCached( this );
}