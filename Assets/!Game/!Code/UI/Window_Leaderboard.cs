using Flexy.Template.BarleyBreak.GameFlow;

namespace Flexy.Template.BarleyBreak.UI
{
	public class Window_Leaderboard : UIWindowEx
	{
		[SerializeField]	BindableDataStore	_recordPrefab;
		[SerializeField]	Transform			_scores_3x3;
		[SerializeField]	Transform			_scores_4x4;
		[SerializeField]	Transform			_scores_5x5;
		[SerializeField]	Transform			_scores_3x3_Container;
		[SerializeField]	Transform			_scores_4x4_Container;
		[SerializeField]	Transform			_scores_5x5_Container;
	
		protected override	void	OnShow	( )		
		{
			ClearContainer	( _scores_3x3_Container );
			ClearContainer	( _scores_4x4_Container );
			ClearContainer	( _scores_5x5_Container );

			_scores_3x3		.gameObject.SetActive( OpenParams is null or EField.Board3x3 );
			_scores_4x4		.gameObject.SetActive( OpenParams is null or EField.Board4x4 );
			_scores_5x5		.gameObject.SetActive( OpenParams is null or EField.Board5x5 );

			gameObject.SetActive( true );

			FillContainer	( _scores_3x3_Container, _recordPrefab, Game.Leaderboards.Leaderboard3X3.Records );
			FillContainer	( _scores_4x4_Container, _recordPrefab, Game.Leaderboards.Leaderboard4X4.Records );
			FillContainer	( _scores_5x5_Container, _recordPrefab, Game.Leaderboards.Leaderboard5X5.Records );
		}

		private		void	ClearContainer	( Transform container )														
		{
			for (var i = container.childCount - 1; i >= 0; i--) 
				Destroy( container.GetChild(i).gameObject );
				
			container.DetachChildren();
		}
		private		void	FillContainer	( Transform container, BindableDataStore prefab, List<Single> collection )	
		{
			if (!container.gameObject.activeInHierarchy)
				return;
		
			foreach (var item in collection)
			{
				var widget = Instantiate(prefab, container);
				
				var seconds	= item;
				var value	= Single.IsPositiveInfinity(seconds) ? "-" : seconds >= 60 
					? TimeSpan.FromSeconds( seconds ).ToString( @"mm\:ss\.ff" ) 
					: TimeSpan.FromSeconds( seconds ).ToString( @"ss\.ff" );
				
				widget.SetValue( "Score", value );
			}
		}

		public record struct Opener( OpenCtx Ctx )
		{
			public	void		Open	( )					=> Ctx.Open( null );
			public	void		Open	( EField board )	=> Ctx.Open( board );
		}
		
		[StateTest]	Object	Board3x3	( ) => EField.Board3x3;
		[StateTest]	Object	Board4x4	( ) => EField.Board4x4;
		[StateTest]	Object	Board5x5	( ) => EField.Board5x5;
	}
}