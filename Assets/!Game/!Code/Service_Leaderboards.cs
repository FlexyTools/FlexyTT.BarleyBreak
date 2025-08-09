namespace Flexy.Template.BarleyBreak
{
	public class Service_Leaderboards: MonoBehaviour, IService
	{
		public void OrderedInit( GameContext ctx )
		{
			Load();
		}
	
		public	BoardData	Leaderboard3X3 {get; private set;}
		public	BoardData	Leaderboard4X4 {get; private set;}
		public	BoardData	Leaderboard5X5 {get; private set;}

		public	void		AddRecord	( EField field, Single score )
		{
			var board = field switch
			{
				EField.Board3x3 => Leaderboard3X3,
				EField.Board4x4 => Leaderboard4X4,
				EField.Board5x5 => Leaderboard5X5,
			};
			
			board.Records.Add( score );
			board.Records.Sort();
			
			while (board.Records.Count > 7)
				board.Records.RemoveAt( board.Records.Count-1 );
			
			Save();
		}
		[ContextMenu("Save")]
		public	void		Save		( )
		{ 
			PlayerPrefs.SetString( "Leaderboard_3x3", JsonUtility.ToJson( Leaderboard3X3 ) );
			PlayerPrefs.SetString( "Leaderboard_4x4", JsonUtility.ToJson( Leaderboard4X4 ) );
			PlayerPrefs.SetString( "Leaderboard_5x5", JsonUtility.ToJson( Leaderboard5X5 ) );
			
			PlayerPrefs.Save( );
		}
		[ContextMenu("Load")]
		public	void		Load		( )
		{
			Leaderboard3X3 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_3x3", "{}" ) );
			Leaderboard4X4 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_4x4", "{}" ) );
			Leaderboard5X5 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_5x5", "{}" ) );
			
			while (Leaderboard3X3.Records.Count < 7) Leaderboard3X3.Records.Add( Single.PositiveInfinity );
			while (Leaderboard4X4.Records.Count < 7) Leaderboard4X4.Records.Add( Single.PositiveInfinity );
			while (Leaderboard5X5.Records.Count < 7) Leaderboard5X5.Records.Add( Single.PositiveInfinity );
		}
		
		[Serializable]
		public class BoardData
		{
			[SerializeField] List<Single> _records;

			public List<Single> Records => _records;
		}
	}
}