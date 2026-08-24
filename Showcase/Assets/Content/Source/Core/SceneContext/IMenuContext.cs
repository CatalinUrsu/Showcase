using Helpers.Audio;
using Helpers.Services;

namespace Source
{
public interface IMenuContext: ISceneContext
{
    IBankLoader BankLoader { get; }
    IPlayerFacade PlayerFacade { get; }
    IMenuUIFacade UIFacade { get; }
    
    void RegisterBankLoader(IBankLoader bankLoader);
    void RegisterPlayerFacade(IPlayerFacade playerFacade);
    void RegisterUIMenuFacade(IMenuUIFacade menuUIFacade);
}
}