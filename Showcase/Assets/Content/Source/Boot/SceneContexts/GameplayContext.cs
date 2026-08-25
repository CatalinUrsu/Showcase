using Helpers.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameplayContext : IGameplayContext
{
#region Fields

    public IBankLoader BankLoader { get; private set; }
    public IPlayerFacade PlayerFacade { get; private set; }
    public IGameUIFacade UIFacade { get; private set; }
    public IEnemiesSpawner EnemiesSpawner { get; private set; }

#endregion

#region Fields Registration

    public void RegisterBankLoader(IBankLoader bankLoader) => BankLoader = bankLoader;
    public void RegisterPlayerFacade(IPlayerFacade playerFacade) => PlayerFacade = playerFacade;
    public void RegisterUIController(IGameUIFacade uiFacade) => UIFacade = uiFacade;
    public void RegisterEnemiesController(IEnemiesSpawner enemiesSpawner) => EnemiesSpawner = enemiesSpawner;

#endregion

#region Public methods

    public async UniTask Init(IProgressTrackingService progressTrackingService, SceneLoadProgress sceneLoadProgress)
    {
        progressTrackingService.UpdateLoadingTip("Setup Gameplay Scene");
        await BankLoader.Init();
        await EnemiesSpawner.Init();
        
        UIFacade.Init();
        PlayerFacade.Init();
    }

    public async UniTask DeInit()
    {
        BankLoader.Deinit();
        PlayerFacade.Deinit();
        UIFacade.Deinit();
        EnemiesSpawner.Deinit();
        
        Clear();
    }

    void Clear()
    {
        BankLoader = null;
        PlayerFacade = null;
        UIFacade = null;
        EnemiesSpawner = null;
    }

#endregion
}
}