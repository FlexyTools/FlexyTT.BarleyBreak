namespace FlexyTT.BarleyBreak.Boot
{
    public class Window_EULA : UIWindowEx
    {
	    private		BooleanSetting	_eulaAccepted	= new("Boot_EulaAccepted", false, readLater:true);
	    public		Boolean			IsDone			=> _eulaAccepted.Read();

	    [Callable]	void			Accept	( )		
        {
			_eulaAccepted.Set(true);
			Close();
        }

	    protected override	void	OnOpen	( FlowNode node )	
	    {
		    base.OnOpen(node);
		    
			if (_eulaAccepted.Read())
				node.Close();
	    }
    }
}