namespace Runner.UI
{
	public class Window_ChooseZone : UIWindowEx
	{
		[SerializeField]	SceneRef	_field_3x3;
		[SerializeField]	SceneRef	_field_4x4;
		[SerializeField]	SceneRef	_field_5x5;
		
		[Callable]	void	Play_Field_3x3		( ) => Game.UI.Play_Field( _field_3x3 );
		[Callable]	void	Play_Field_4x4		( ) => Game.UI.Play_Field( _field_4x4 );
		[Callable]	void	Play_Field_5x5		( ) => Game.UI.Play_Field( _field_5x5 );
	}
}