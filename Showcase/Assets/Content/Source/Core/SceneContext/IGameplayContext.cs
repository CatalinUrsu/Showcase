using Helpers.Audio;
using Helpers.Services;

namespace Source
{
public interface IGameplayContext: ISceneContext
{
    IBankLoader BankLoader { get; }
    IPlayerFacade PlayerFacade { get; }
    IGameUIFacade UIFacade { get; }
    IEnemiesSpawner EnemiesSpawner { get; }
    
    void RegisterBankLoader(IBankLoader bankLoader);
    void RegisterPlayerFacade(IPlayerFacade playerFacade);
    void RegisterUIController(IGameUIFacade uiFacade);
    void RegisterEnemiesController(IEnemiesSpawner enemiesSpawner);
}
}