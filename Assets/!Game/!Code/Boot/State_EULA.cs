namespace FlexyTemplates.BarleyBreak.Boot
{
    public class State_EULA : StateEx, IBootState
    {
	    private BooleanSetting	_eulaAccepted	= new("Boot_EulaAccepted", false, readLater:true);

	    protected override void OnShow()
	    {
			_eulaAccepted.Read();
	    
		    base.OnShow();
	    }

	    [Callable]	void	Accept	( )	
        {
			_eulaAccepted.Set(true);
			Close();
        }

	    public Boolean IsDone
	    {
			get
			{
				_eulaAccepted.Read();
				return _eulaAccepted.Get();
			}
	    }
    }
}