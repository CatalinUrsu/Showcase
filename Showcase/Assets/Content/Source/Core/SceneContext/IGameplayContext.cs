using Helpers.Audio;

namespace Source
{
public interface IGameplayContext: ISceneContext
{
    IBankLoader BankLoader { get; }
    IPlayerFacade PlayerFacade { get; }
    IGameUIFacade UIFacade { get; }
    IEnemiesController EnemiesController { get; }
    IGameRunModelController GameRunModelController { get; }
    
    void RegisterBankLoader(IBankLoader bankLoader);
    void RegisterPlayerFacade(IPlayerFacade playerFacade);
    void RegisterUIController(IGameUIFacade uiFacade);
    void RegisterEnemiesController(IEnemiesController enemiesController);
    void RegisterGameRunModelController(IGameRunModelController gameRunModelController);
}
}