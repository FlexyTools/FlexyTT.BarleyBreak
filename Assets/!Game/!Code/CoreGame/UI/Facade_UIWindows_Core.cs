// ReSharper disable AccessToStaticMemberViaDerivedType

using Flexy.Template.BarleyBreak.UI;
using Flexy.UI.Extra;

namespace Flexy.Template.BarleyBreak.CoreGame.UI;

public readonly record struct Facade_UIWindows_Core( FlowLib Lib )
{
	public Window_Pause				.Opener		Pause				=> Lib.GetState<Window_Pause>( );
	public Window_FieldComplete		.Opener		FieldComplete		=> Lib.GetOpener<Window_FieldComplete.Opener>( );
	public Window_Leaderboard		.Opener		Leaderboards		=> Lib.GetOpener<Window_Leaderboard.Opener>( );
	
	public Window_GameSettings		.Opener		GameSettings		=> Lib.GetState<Window_GameSettings>( );
	public Window_FreeCam			.Opener		FreeCam				=> Lib.GetState<Window_FreeCam>( );
}