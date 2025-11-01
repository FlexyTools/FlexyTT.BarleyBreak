namespace FlexyTemplates.BarleyBreak.Metagame
{
	public class Window_PlayFields : UIWindowEx
	{
		[SerializeField]	SceneRef	_field_3x3;
		[SerializeField]	SceneRef	_field_4x4;
		[SerializeField]	SceneRef	_field_5x5;
		
		[Callable]	void	Play_Field_3x3		( ) => Game.Flow.Play_Field( _field_3x3 ).Forget();
		[Callable]	void	Play_Field_4x4		( ) => Game.Flow.Play_Field( _field_4x4 ).Forget();
		[Callable]	void	Play_Field_5x5		( ) => Game.Flow.Play_Field( _field_5x5 ).Forget();
		
		[Callable]	void	OpenLeaderboards	( )		
		{
			Game.UI.Leaderboards.Open( );
		}
	}
}