using Helpers.Audio;

namespace Source
{
public interface IMenuContext: ISceneContext
{
    IBankLoader BankLoader { get; }
    IPlayerFacade PlayerFacade { get; }
    IUIMenuFacade UIMenuFacade { get; }
    
    void RegisterBankLoader(IBankLoader bankLoader);
    void RegisterPlayerFacade(IPlayerFacade playerFacade);
    void RegisterUIMenuFacade(IUIMenuFacade uiMenuFacade);
}
}