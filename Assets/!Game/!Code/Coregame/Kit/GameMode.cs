namespace FlexyTemplates.BarleyBreak.Coregame.Kit
{
	public class GameMode : MonoBehEx
	{
		[SerializeField]	EField			_board;
		[SerializeField]	Cell			_cellPrefab		= null!;
		[SerializeField]	GameObject		_inputBlocker	= null!;
		[SerializeField]	GridLayoutGroup	_cellContainer	= null!;
		[SerializeField]	Int32			_gridSize		= 3;

		[SerializeField]	FlexyEvent		_win;
	
		private		List<Cell>	_cells		= new();

		public		EField		Board		=> _board;
		public		Single		StartTime	{ get; set; }
		public		Single		RunTime		=> Time.time - StartTime;
		public		Boolean		IsWin		{ get; set; }
		
		public		Single		Result		{ get; private set; }

		public		void	StartPlay		( )		
		{
			foreach (var cell in _cells)
				cell.ResetCell();
        
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
					if (x<0 | y<0 | x>=_gridSize | y>=_gridSize) 
						return curr;

					var targetIndex = y * _gridSize + x;
					MoveFigureTo( _cells[targetIndex], curr, false );
					
					return _cells[targetIndex];
				}
			}
			
			StartTime = Time.time;
		}

		private     void	Awake			( )     
		{
			_cellPrefab.gameObject.SetActive(false);
			_cellContainer.constraintCount = _gridSize;
        
			for (var i = 0; i < _gridSize*_gridSize; i++)
			{
				var cell = Instantiate( _cellPrefab, _cellContainer.transform );
                
				cell.GameMode = this;
				cell.Index = i;
                
				_cells.Add( cell );
				cell.gameObject.SetActive(true);
			}
            
			_cells[^1].MakeLastCellClear();
		}
		private		void	Update			( )		
		{
			#if UNITY_EDITOR
			if (Keyboard.current.wKey.wasPressedThisFrame)
				Win();
			#endif
		}

		internal	void	ClickCell		( Int32 cellIndex, Boolean animate = true )						
		{
			var cellX = cellIndex % _gridSize;
			var cellY = cellIndex / _gridSize;
        
			Check( cellX-1, cellY, animate );
			Check( cellX+1, cellY, animate );
			Check( cellX, cellY-1, animate );
			Check( cellX, cellY+1, animate );
            
			if (CheckWin())
			{
				_inputBlocker.SetActive( true );
				Win();
			}
            
			return;

			void Check( Int32 x, Int32 y, Boolean anim )
			{
				if (x<0 | y<0 | x>=_gridSize | y>=_gridSize) 
					return;

				var index = y * _gridSize + x;
				    
				if (_cells[index].IsEmpty)
				{
					MoveFigureTo( _cells[cellIndex], _cells[index], anim );
				}
			}
		}
		private		void	MoveFigureTo	( Cell sourceCell, Cell targetCell, Boolean animate = true )	
		{
			var figure		= sourceCell.Figure;
        
			if (figure == null)
				throw new ArgumentNullException("sourceCell.Figure is null");
        
			figure.SetParent(targetCell.transform, true);
			if (!animate)
				figure.position	= targetCell.GlobalPosition;
				
			targetCell.Figure = figure;
			sourceCell.Figure = null;
		}
	
		private		Boolean	CheckWin		( )		
		{
			for (var i = 0; i < _cells.Count - 1; i++)
			{
				if (_cells[i].IsEmpty || !_cells[i].IsSolved)
					return false;
			}

			return true;
		}
		private		void	Win				( )		
		{
			IsWin = true;
			Result = RunTime;
			_win.Raise( this );
		} 
	}
}