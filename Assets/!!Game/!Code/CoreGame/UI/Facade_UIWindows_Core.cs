// ReSharper disable AccessToStaticMemberViaDerivedType

using Flexy.UI.Extra;
using Runner.UI;

namespace Test.Runner.CoreGame.UI;

public readonly record struct Facade_UIWindows_Core( FlowLib Lib )
{
	public Window_Pause				.Opener		Pause				=> Lib.GetState<Window_Pause>( );
	public Window_GameOver			.Opener		GameOver			=> Lib.GetOpener<Window_GameOver.Opener>( );
	
	public Window_GameSettings		.Opener		GameSettings		=> Lib.GetState<Window_GameSettings>( );
	public Window_FreeCam			.Opener		FreeCam				=> Lib.GetState<Window_FreeCam>( );
}