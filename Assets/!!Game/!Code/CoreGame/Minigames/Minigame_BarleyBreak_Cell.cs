using UnityEngine.Serialization;

namespace Flexy.Template.BarleyBreak.CoreGame.Minigames
{
    public class Minigame_BarleyBreak_Cell : MonoBehEx
    {
        [FormerlySerializedAs("_visualItem")] 
        [SerializeField]    RectTransform   _figure;

        private RectTransform _rectTransform;
        public Int32 Index { get; set; }

		[Bindable]	Int32	Number => Index + 1;

        public Minigame_BarleyBreak Game { get; set; }
        public RectTransform Figure
        {
	        get => _figure;
	        set => _figure = value;
        }

        public Boolean IsEmpty => _figure == null;
        public Vector3 GlobalPosition => _rectTransform.position;

        private		void	Awake		( )	
        {
	        _rectTransform = GetComponent<RectTransform>();
        }

        [Callable]	void	ClickCell	( )	
        {
            Game.ClickCell( Index );
        }
    }
}