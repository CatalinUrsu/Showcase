using Helpers;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class LoadGameplayState : StateBase
{
    IGameplayContext _gameplayContext;
    
#region Public methods

    public LoadGameplayState(ISceneLoaderService sceneLoaderService,
                             IProgressTrackingService progressTrackingService,
                             ISplashScreen splashScreen,
                             IAudioService audioService,
                             IGameplayContext gameplayContext)
        : base(sceneLoaderService, progressTrackingService, splashScreen, audioService)
    {
        _gameplayContext = gameplayContext;
    }

    public override async UniTask Enter()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            await LoadGameplayScene();
            await HideSplashScreen();
            await StatesMachine.Enter<GameplayState>();
        }
    }

    public override UniTask Exit() => UniTask.CompletedTask;

#endregion

#region Private methods

    async UniTask LoadGameplayScene()
    {
        var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.GAME_SCENE)
                              .SetTip("Load Gameplay Scene")
                              .SetIsAddressable(true)
                              .SetActiveOnLoad(true)
                              .Build();
        
        await LoadScene(sceneLoadParams);
    }

    protected override async UniTask InitSceneContext(SceneLoadProgress sceneLoadProgress)
    {
        _progressTrackingService.UpdateLoadingTip("Setup Gameplay Scene");
        
        _gameplayContext.UIFacade.Init();
        _gameplayContext.PlayerFacade.Init();
        SetMusicState(EMusicStates.Gameplay);

        await UniTask.WhenAll(_gameplayContext.BankLoader.Init(),
                              _gameplayContext.EnemiesSpawner.Init());

        sceneLoadProgress.SetupProgress = 1;
    }
    
#endregion    
}
}