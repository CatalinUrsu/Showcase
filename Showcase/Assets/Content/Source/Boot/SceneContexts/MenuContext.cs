using Helpers.Audio;

namespace Source.Boot
{
public class MenuContext : IMenuContext
{
    public IBankLoader BankLoader { get; private set; }
    public IPlayerFacade PlayerFacade { get; private set; }
    public IUIMenuFacade UIMenuFacade { get; private set; }

    public void RegisterBankLoader(IBankLoader bankLoader) => BankLoader = bankLoader;

    public void RegisterPlayerFacade(IPlayerFacade playerFacade) => PlayerFacade = playerFacade;

    public void RegisterUIMenuFacade(IUIMenuFacade uiMenuFacade) => UIMenuFacade = uiMenuFacade;

    public void Clear()
    {
        BankLoader = null;
        PlayerFacade = null;
        UIMenuFacade = null;
    }
}
}