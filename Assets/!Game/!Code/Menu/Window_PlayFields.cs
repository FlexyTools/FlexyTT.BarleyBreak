namespace FlexyTT.BarleyBreak.Menu
{
	public class Window_PlayFields : UIWindowEx
	{
		[SerializeField]	SceneRef	_field_3x3;
		[SerializeField]	SceneRef	_field_4x4;
		[SerializeField]	SceneRef	_field_5x5;
		
		[Callable]	void	Play_Field_3x3		( ) => Game.Flow.PlayField( _field_3x3 );
		[Callable]	void	Play_Field_4x4		( ) => Game.Flow.PlayField( _field_4x4 );
		[Callable]	void	Play_Field_5x5		( ) => Game.Flow.PlayField( _field_5x5 );
		
		[Callable]	void	OpenLeaderboards	( )		
		{
			Game.UI.Leaderboards.Open( );
		}
	}
}