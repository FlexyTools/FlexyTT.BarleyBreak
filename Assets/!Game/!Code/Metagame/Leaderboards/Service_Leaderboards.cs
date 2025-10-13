namespace FlexyTemplates.BarleyBreak.Metagame.Leaderboards
{
	public class Service_Leaderboards : MonoBehaviour, IService
	{
		public void OrderedInit( GameContext ctx )
		{
			Load();
		}
	
		public	BoardData	Board3X3	{get; private set;} = null!;
		public	BoardData	Board4X4	{get; private set;} = null!;
		public	BoardData	Board5X5	{get; private set;} = null!;

		public	void		AddRecord	( EField field, Single score )	
		{
			var board = field switch
			{
				EField.Board3x3 => Board3X3,
				EField.Board4x4 => Board4X4,
				EField.Board5x5 => Board5X5,
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
			PlayerPrefs.SetString( "Leaderboard_3x3", JsonUtility.ToJson( Board3X3 ) );
			PlayerPrefs.SetString( "Leaderboard_4x4", JsonUtility.ToJson( Board4X4 ) );
			PlayerPrefs.SetString( "Leaderboard_5x5", JsonUtility.ToJson( Board5X5 ) );
			
			PlayerPrefs.Save( );
		}
		[ContextMenu("Load")]
		public	void		Load		( )	
		{
			Board3X3 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_3x3", "{}" ) );
			Board4X4 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_4x4", "{}" ) );
			Board5X5 = JsonUtility.FromJson<BoardData>( PlayerPrefs.GetString( "Leaderboard_5x5", "{}" ) );
			
			while (Board3X3.Records.Count < 7) Board3X3.Records.Add( Single.PositiveInfinity );
			while (Board4X4.Records.Count < 7) Board4X4.Records.Add( Single.PositiveInfinity );
			while (Board5X5.Records.Count < 7) Board5X5.Records.Add( Single.PositiveInfinity );
		}
		
		[Serializable]
		public class BoardData
		{
			[SerializeField] List<Single> _records = new();

			public List<Single> Records => _records;
		}
	}
}