namespace FlexyTemplates.BarleyBreak.Boot
{
	public class Window_AgeSelect : UIWindowEx, IBootState
	{
		private		Int32			_age		= 12;
		private		Int32Setting	_ageSetting	= new("Boot_SelectedAge", 0, readLater:true);
		public		Boolean			IsDone		=> _ageSetting.Read() != 0;

		[Bindable]	Single		Age			
		{ 
			get => _age; 
			set 
			{ 
				_age = (Int32)value; 
				RebindProperty( "Age" );
				RebindProperty( "AgeString" ); 
			} 
		}
		[Bindable]	String		AgeString	=> Age.ToString(CultureInfo.InvariantCulture);
		[Callable]	void		Accept		( )		
		{
			_ageSetting.Set(_age);
			Close();
		}
	
		protected override void	OnShow		( )		
		{
			_ageSetting.Read();
	    
			base.OnShow();
		}
	}
}