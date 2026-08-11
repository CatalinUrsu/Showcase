using Helpers.Audio;

namespace Source.Boot
{
public class GameplayContext : IGameplayContext
{
    public IBankLoader BankLoader { get; private set; }
    public IPlayerFacade PlayerFacade { get; private set; }
    public IGameUIFacade UIFacade { get; private set; }
    public IEnemiesSpawner EnemiesSpawner { get; private set; }
    public IGameRunModelController GameRunModelController { get; private set; }

    public void RegisterBankLoader(IBankLoader bankLoader) => BankLoader = bankLoader;
    public void RegisterPlayerFacade(IPlayerFacade playerFacade) => PlayerFacade = playerFacade;
    public void RegisterUIController(IGameUIFacade uiFacade) => UIFacade = uiFacade;
    public void RegisterEnemiesController(IEnemiesSpawner enemiesSpawner) => EnemiesSpawner = enemiesSpawner;
    public void RegisterGameRunModelController(IGameRunModelController gameRunModelController) => GameRunModelController = gameRunModelController;

    public void Clear()
    {
        BankLoader = null;
        PlayerFacade = null;
        UIFacade = null;
        EnemiesSpawner = null;
        GameRunModelController = null;
    }
}
}