namespace Flexy.Template.BarleyBreak.CoreGame.Minigames
{
    public class Minigame_BarleyBreak_Cell : MonoBehEx
    {
        [FormerlySerializedAs("_visualItem")] 
        [SerializeField]    RectTransform   _figure;

        private RectTransform _originalFigure;
        private RectTransform _rectTransform;
        public Int32 Index { get; set; }

		[Bindable]	Int32	Number => Index + 1;

        public Minigame_BarleyBreak Game { get; set; }
        public RectTransform Figure
        {
	        get => _figure;
	        set => _figure = value;
        }

        public Boolean IsEmpty			=> _figure == null;
        public Boolean IsSolved			=> _figure == _originalFigure;
        public Vector3 GlobalPosition	=> _rectTransform.position;

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

        [Callable]	void	ClickCell	( )	
        {
            Game.ClickCell( Index );
        }
    }
}