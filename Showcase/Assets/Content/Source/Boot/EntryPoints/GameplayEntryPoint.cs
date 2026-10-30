using UnityEngine;
using Helpers.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameplayEntryPoint : IGameplayEntryPoint
{
#region Fields

    public IPlayerFacade PlayerFacade { get; }
    public IGameUIFacade UIFacade { get; }
    public IEnemiesSpawner EnemiesSpawner { get; }
    public Collider2D GameCameraBounds { get; }

    readonly IBankLoader _bankLoader;

#endregion

#region Public methods

    public GameplayEntryPoint(IBankLoader bankLoader,
                              IPlayerFacade playerFacade,
                              IGameUIFacade uiFacade,
                              IEnemiesSpawner enemiesSpawner,
                              Collider2D gameCameraBounds)
    {
        _bankLoader = bankLoader;
        PlayerFacade = playerFacade;
        UIFacade = uiFacade;
        EnemiesSpawner = enemiesSpawner;
        GameCameraBounds = gameCameraBounds;
    }

    public async UniTask Init(IProgressTrackingService progressTrackingService, SceneLoadProgress sceneLoadProgress)
    {
        progressTrackingService.UpdateLoadingTip("Setup Gameplay Scene");
        await _bankLoader.Init();
        await EnemiesSpawner.Init();
        
        UIFacade.Init();
        PlayerFacade.Init();
    }

    public UniTask DeInit()
    {
        _bankLoader.Deinit();
        PlayerFacade.Deinit();
        UIFacade.Deinit();
        EnemiesSpawner.Deinit();
        return UniTask.CompletedTask;
    }

#endregion
}
}
