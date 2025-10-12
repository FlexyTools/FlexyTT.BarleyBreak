namespace Flexy.Template.BarleyBreak.Coregame
{
    public class Cell : MonoBehEx
    {
        [SerializeField]    RectTransform   _figure;
        [SerializeField]    Single			_figureAnimationSpeed = 18;

        private RectTransform _originalFigure;
        private RectTransform _rectTransform;
        
        internal	Int32	Index	{ get; set; }
        
		[Bindable]	String	Number	=> (Index + 1).ToString();

        public	GameMode		Game	{ get; set; }
        public	RectTransform	Figure	
        {
	        get => _figure;
	        set => _figure = value;
        }

        public		Boolean	IsEmpty			=> _figure == null;
        public		Boolean	IsSolved		=> _figure == _originalFigure;
        public		Vector3	GlobalPosition	=> _rectTransform.position;

        public		void	MakeLastCellClear	( )	
        {
	        Destroy(Figure.gameObject);
	        Figure = null;
	        _originalFigure = null;
        }
        public		void	ResetCell			( )	
        {
			_figure				= _originalFigure;
	        
	        if (_figure)
	        {
		        _figure.SetParent	( transform );
		        _figure.position	= GlobalPosition;
	        }
        }

        private		void	Awake		( )	
        {
	        _rectTransform	= GetComponent<RectTransform>();
	        _originalFigure	= _figure;
        }
        private		void	Update		( )	
        {
            if (_figure)
                _figure.anchoredPosition = ExpLerp(_figure.anchoredPosition, Vector2.zero, _figureAnimationSpeed, Time.deltaTime);
        }

        private		Vector2	ExpLerp		( Vector2 current, Vector2 target, Single speed, Single deltaTime )	
        {
	        var t = 1f - Mathf.Exp(-speed * deltaTime);
	        return Vector2.Lerp(current, target, t);
        }

        [Callable]	void	ClickCell	( )	
        {
            Game.ClickCell( Index );
        }
    }
}