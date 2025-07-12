namespace Flexy.Template.BarleyBreak.UI
{
	public class Service_Leaderboards: MonoBehaviour, IService
	{
		public void OrderedInit( GameContext ctx )
		{
			Load();	
		}
	
		[SerializeField]	BoardData	_leaderboard3x3;
		[SerializeField]	BoardData	_leaderboard4x4;
		[SerializeField]	BoardData	_leaderboard5x5;

		public	BoardData	Leaderboard3X3 => _leaderboard3x3;
		public	BoardData	Leaderboard4X4 => _leaderboard4x4;
		public	BoardData	Leaderboard5X5 => _leaderboard5x5;

		public	void		AddRecord	( EField field, Single score )
		{
			var board = field switch
			{
				EField.Board3x3 => _leaderboard3x3,
				EField.Board4x4 => _leaderboard4x4,
				EField.Board5x5 => _leaderboard5x5,
			};
			
			board.Records.Add( score );
			board.Records.Sort( );
			
			while (board.Records.Count > 7)
				board.Records.RemoveAt( board.Records.Count-1 );
			
			Save( );
		}
		[ContextMenu("Save")]
		public	void		Save		( )
		{ 
			PlayerPrefs.SetString( "Leaderboard_3x3", JsonUtility.ToJson( _leaderboard3x3 ) );
			PlayerPrefs.SetString( "Leaderboard_4x4", JsonUtility.ToJson( _leaderboard4x4 ) );
			PlayerPrefs.SetString( "Leaderboard_5x5", JsonUtility.ToJson( _leaderboard5x5 ) );
			
			PlayerPrefs.Save( );
		}
		[ContextMenu("Load")]
		public	void		Load		( )
		{
			_leaderboard3x3 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_3x3", "{}" ) );
			_leaderboard4x4 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_4x4", "{}" ) );
			_leaderboard5x5 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_5x5", "{}" ) );
			
			while (_leaderboard3x3.Records.Count < 7) _leaderboard3x3.Records.Add( Single.PositiveInfinity );
			while (_leaderboard4x4.Records.Count < 7) _leaderboard4x4.Records.Add( Single.PositiveInfinity );
			while (_leaderboard5x5.Records.Count < 7) _leaderboard5x5.Records.Add( Single.PositiveInfinity );
		}
		
		[Serializable]
		public class BoardData
		{
			[SerializeField] List<Single> _records;

			public List<Single> Records => _records;
		}
	}
}