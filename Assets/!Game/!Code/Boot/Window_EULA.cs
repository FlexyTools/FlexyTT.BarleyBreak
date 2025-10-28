namespace FlexyTemplates.BarleyBreak.Boot
{
    public class Window_EULA : StateEx, IBootState
    {
	    private		BooleanSetting	_eulaAccepted	= new("Boot_EulaAccepted", false, readLater:true);
	    public		Boolean			IsDone			=> _eulaAccepted.Read();

	    [Callable]	void		Accept	( )		
        {
			_eulaAccepted.Set(true);
			Close();
        }
	    protected override void	OnShow	( )		
	    {
		    _eulaAccepted.Read();
	    
		    base.OnShow();
	    }
    }
}