using FlexyTemplates.BarleyBreak.Metagame;

namespace FlexyTemplates.BarleyBreak.Boot;

public abstract class	GameStageEx: GameStage					
{
	private		Facade_Game	_game; 
	public	ref Facade_Game	Game	=> ref _game.GetCached( this );
}

public abstract class	UIWindowEx: UIWindow					
{
	private		Facade_Game	_game; 
	public	ref Facade_Game	Game	=> ref _game.GetCached( this );
}