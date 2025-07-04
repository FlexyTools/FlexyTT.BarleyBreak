namespace Flexy.Template.BarleyBreak.CoreGame.Minigames
{
    public class Minigame_BarleyBreak : MonoBehEx
    {
        [SerializeField]	Minigame_BarleyBreak_Cell _cellPrefab;
        [SerializeField]	GameObject		_inputBlocker;
        [SerializeField]	GridLayoutGroup	_cellContainer;
        [SerializeField]	Int32			_gridSize = 3;
	
        private readonly	AnimationCurve	_animCurve = AnimationCurve.EaseInOut( 0, 0, 1, 1 );
	
	    private List<Minigame_BarleyBreak_Cell> _cells = new();
	
        public Single RunTime { get; }

        private     void    Awake       ( )     
        {
	        _cellPrefab.gameObject.SetActive(false);
	        _cellContainer.constraintCount = _gridSize;
        
            for (var i = 0; i < _gridSize*_gridSize; i++)
            {
                var cell = Instantiate( _cellPrefab, _cellContainer.transform );
                
                cell.Game = this;
                cell.Index = i;
                
                _cells.Add( cell );
                cell.gameObject.SetActive(true);
            }
            
            // Clear last cell
            var f = _cells[^1].Figure;
            _cells[^1].Figure = null;
            Destroy(f.gameObject);
        }

        private		void	OnEnable	( )		
        {
            InitMinigameAsync( ).Forget( );
        }

        internal	void	ClickCell	( Int32 cellIndex )				
        {
            var x = cellIndex % _gridSize;
            var y = cellIndex / _gridSize;
        
            Check( x-1, y );
            Check( x+1, y );
            Check( x, y-1 );
            Check( x, y+1 );
            
            return;

            void Check( Int32 x, Int32 y )
            {
				if (x<0|y<0|x>=_gridSize|y>=_gridSize) 
					return;

				var index = y * _gridSize + x;
				    
				if (_cells[index].IsEmpty)
				{
					MoveFigureTo( _cells[cellIndex], index );
				}
            }
        }
        private		void	MoveFigureTo		( Minigame_BarleyBreak_Cell sourceCell, Int32 to, Boolean animate = true )	
        {
			var targetCell = _cells[to];
        
            if (animate)
				AnimateFigureTo( sourceCell, targetCell, 0.4f ).Forget( );
				
            targetCell.Figure = sourceCell.Figure;
            sourceCell.Figure = null;
        }
	
        private async	UniTask		InitMinigameAsync	( )		
        {
            // for (var i = 0; i < _cups.Length; i++)
            //     if( _cups[i] )
            //         AnimateFigureTo( _cups[i], _cells[i].anchoredPosition).Forget( );
        }
        private async	UniTask		AnimateFigureTo		( Minigame_BarleyBreak_Cell from, Minigame_BarleyBreak_Cell to, Single animationDuration = 1 ) 
        {
			var figure = from.Figure;
	        figure.SetParent(from.transform.parent, true);
        
            var fromAnchoredPosition = from.GlobalPosition;
            var toAnchoredPosition = to.GlobalPosition;
            
            var elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                var t = _animCurve.Evaluate(elapsedTime / animationDuration);

                figure.position	= Vector2.Lerp(fromAnchoredPosition, toAnchoredPosition, t);
	        
                elapsedTime += Time.deltaTime;
                await UniTask.NextFrame( );
            }

            figure.position	= to.GlobalPosition;
			figure.SetParent(to.transform, true);
		
            // if( target == _cells[8].anchoredPosition )
            //     DoWinSequenseAsync( ).Forget( );
        }
        private async	UniTask		DoWinSequenseAsync	( )	    
        {
            _inputBlocker.SetActive( true );
			
            // Tween.Value( 0.01f, 1, 0.5f ).BindTo( _hole, static (h, scale) => h.localScale = new(scale, scale, scale) ).Run( );
			         //
            // await UniTask.Delay( 700 );
			         //
            // Tween.LocalPositionY( _mainCup, _mainCup.localPosition.y, _mainCup.localPosition.y -400, 0.3f ).Run( );
			         //
            // await UniTask.Delay( 400 );
		
            gameObject.SetActive( false );
        }
    }
}