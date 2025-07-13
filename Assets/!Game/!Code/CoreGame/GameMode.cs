using Flexy.Core.Tweens;

namespace Flexy.Template.BarleyBreak.CoreGame
{
    public class GameMode : MonoBehEx
    {
	    [SerializeField]	EField			_board;
        [SerializeField]	Cell			_cellPrefab;
        [SerializeField]	GameObject		_inputBlocker;
        [SerializeField]	GridLayoutGroup	_cellContainer;
        [SerializeField]	Int32			_gridSize = 3;
        [SerializeField]	Single			_animateTime = 0.2f;
        
        [SerializeField]	FlexyEvent		_win;
	
	    private List<Cell> _cells = new();

	    public		EField	Board		=> _board;
	    public		Single	StartTime	{ get; set; }
        public		Single	RunTime		=> Time.realtimeSinceStartup - StartTime;
        public		Boolean	IsWin		{ get; set; }

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
            
            _cells[^1].MakeLastCellClear();
        }
        private		void	OnEnable	( )		
        {
            InitMinigameAsync( ).Forget( );
        }
        
        #if UNITY_EDITOR
        private		void	Update		( )		
        {
	        if (Input.GetKeyDown(KeyCode.W))
		        Win( );
        }
        #endif

        internal	void	ClickCell		( Int32 cellIndex, Boolean animate = true )															
        {
            var x = cellIndex % _gridSize;
            var y = cellIndex / _gridSize;
        
            Check( x-1, y, animate );
            Check( x+1, y, animate );
            Check( x, y-1, animate );
            Check( x, y+1, animate );
            
            if (CheckWin( ))
			{
				_inputBlocker.SetActive( true );
				Win( );
			}
            
            return;

            void Check( Int32 x, Int32 y, Boolean animate )
            {
				if (x<0|y<0|x>=_gridSize|y>=_gridSize) 
					return;

				var index = y * _gridSize + x;
				    
				if (_cells[index].IsEmpty)
				{
					MoveFigureTo( _cells[cellIndex], _cells[index], animate );
				}
            }
        }
        private		void	MoveFigureTo	( Cell sourceCell, Cell targetCell, Boolean animate = true )	
        {
			var figure		= sourceCell.Figure;
        
            if (animate)
            {
				AnimateFigureTo( sourceCell, targetCell, _animateTime ).Forget( );
			}
			else
			{
				figure.SetParent(targetCell.transform, true);
				figure.position	= targetCell.GlobalPosition;
			}
				
            targetCell.Figure = figure;
            sourceCell.Figure = null;
        }
	
        private async	UniTask		InitMinigameAsync	( )		
        {
	        foreach (var cell in _cells)
		        cell.ResetCell( );
        
			//Time.timeScale = 10;
        
			var currEmptyCell = _cells[^1];
            for (var i = 0; i < 500; i++)
            {
	            var dir = Random.Range(0, 4);
            
	            var x = currEmptyCell.Index % _gridSize;
	            var y = currEmptyCell.Index / _gridSize;
        
				switch(dir)
				{
					case 0: currEmptyCell = Check( currEmptyCell, x-1, y ); break;
					case 1: currEmptyCell = Check( currEmptyCell, x+1, y ); break;
					case 2: currEmptyCell = Check( currEmptyCell, x, y-1 ); break;
					case 3: currEmptyCell = Check( currEmptyCell, x, y+1 ); break;
				}
        
				Cell Check( Cell curr, Int32 x, Int32 y )
				{
					if (x<0|y<0|x>=_gridSize|y>=_gridSize) 
						return curr;

					var targetIndex = y * _gridSize + x;
					MoveFigureTo( _cells[targetIndex], curr, false );
					
					return _cells[targetIndex];
				}
			}
			
			StartTime = Time.realtimeSinceStartup;
        }
        private async	UniTask		AnimateFigureTo		( Cell from, Cell to, Single animationDuration = 1 ) 
        {
			var figure = from.Figure;
	        figure.SetParent(from.transform.parent, true);
        
            var fromAnchoredPosition = from.GlobalPosition;
            var toAnchoredPosition = to.GlobalPosition;
            
            var endTime = Time.time + animationDuration;
            
            while (Time.time < endTime)
            {
	            var t = 1.0f - (endTime - Time.time) / animationDuration; 
				t = EaseUtility.SCurve(t);
            
                figure.position	= Vector2.Lerp(fromAnchoredPosition, toAnchoredPosition, t);
	        
                await UniTask.NextFrame( );
            }

            figure.position	= to.GlobalPosition;
			figure.SetParent(to.transform, true);
        }
        
        private			Boolean		CheckWin			( )		
        {
            for (var i = 0; i < _cells.Count - 1; i++)
            {
                if (_cells[i].IsEmpty || !_cells[i].IsSolved)
                    return false;
            }

            return true;
        }
        private			void		Win					( )		
        {
	        IsWin = true;
	        _win.Raise( this );
        } 
    }
}