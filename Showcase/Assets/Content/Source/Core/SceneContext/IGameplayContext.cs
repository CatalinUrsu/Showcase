using Helpers.Audio;

namespace Source
{
public interface IGameplayContext: ISceneContext
{
    IBankLoader BankLoader { get; }
    IPlayerFacade PlayerFacade { get; }
    
    void RegisterBankLoader(IBankLoader bankLoader);
    void RegisterPlayerFacade(IPlayerFacade playerFacade);
}
}