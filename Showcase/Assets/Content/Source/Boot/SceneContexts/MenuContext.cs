using Helpers.Audio;

namespace Source.Boot
{
public class MenuContext : IMenuContext
{
    public IBankLoader BankLoader { get; private set; }
    public IPlayerFacade PlayerFacade { get; private set; }
    public IMenuUIFacade UIFacade { get; private set; }

    public void RegisterBankLoader(IBankLoader bankLoader) => BankLoader = bankLoader;

    public void RegisterPlayerFacade(IPlayerFacade playerFacade) => PlayerFacade = playerFacade;

    public void RegisterUIMenuFacade(IMenuUIFacade menuUIFacade) => UIFacade = menuUIFacade;

    public void Clear()
    {
        BankLoader = null;
        PlayerFacade = null;
        UIFacade = null;
    }
}
}