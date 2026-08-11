using Helpers.Audio;

namespace Source
{
public interface IGameplayContext: ISceneContext
{
    IBankLoader BankLoader { get; }
    IPlayerFacade PlayerFacade { get; }
    IGameUIFacade UIFacade { get; }
    IEnemiesSpawner EnemiesSpawner { get; }
    IGameRunModelController GameRunModelController { get; }
    
    void RegisterBankLoader(IBankLoader bankLoader);
    void RegisterPlayerFacade(IPlayerFacade playerFacade);
    void RegisterUIController(IGameUIFacade uiFacade);
    void RegisterEnemiesController(IEnemiesSpawner enemiesSpawner);
    void RegisterGameRunModelController(IGameRunModelController gameRunModelController);
}
}