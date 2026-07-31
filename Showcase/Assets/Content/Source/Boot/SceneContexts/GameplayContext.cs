using Helpers.Audio;

namespace Source.Boot
{
public class GameplayContext : IGameplayContext
{
    public IBankLoader BankLoader { get; private set; }
    public IPlayerFacade PlayerFacade { get; private set; }

    public void RegisterBankLoader(IBankLoader bankLoader) => BankLoader = bankLoader;

    public void RegisterPlayerFacade(IPlayerFacade playerFacade) => PlayerFacade = playerFacade;

    public void Clear()
    {
        BankLoader = null;
        PlayerFacade = null;
    }
}
}