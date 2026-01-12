namespace FlexyTT.BarleyBreak.Boot
{
    public class Window_EULA : UIWindowEx, IBootState
    {
	    private		BooleanSetting	_eulaAccepted	= new("Boot_EulaAccepted", false, readLater:true);
	    public		Boolean			IsDone			=> _eulaAccepted.Read();

	    [Callable]	void			Accept	( )		
        {
			_eulaAccepted.Set(true);
			Close();
        }
	    protected override UniTask	OnShow	( )		
	    {
		    _eulaAccepted.Read();
	    
		    return default;
	    }
    }
}