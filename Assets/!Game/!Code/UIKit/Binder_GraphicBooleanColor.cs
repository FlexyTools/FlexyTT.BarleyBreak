namespace FlexyTT.BarleyBreak.UIKit
{
	[BindTo(typeof(Boolean))]
	public class Binder_GraphicBooleanColor : Binder
	{
		[Tooltip("Will get from this GO, if not set")]
		[SerializeField] Graphic _widget = null!;
		
		[SerializeField] Color _true;
		[SerializeField] Color _false;
	
		private Boolean			_value;
		private Func<Boolean>	_getter = null!;

		private				void	Awake	( )					
		{
			if (_widget == null)
				_widget = GetComponent<Graphic>();

			Init(ref _getter);
		}
		protected override	void	Bind	( Boolean init )	
		{
			var color = _getter();
    
			if (!init && _value == color)
				return;
	
			_value = color;
			_widget.color = color ? _true : _false;
		}
	}
}