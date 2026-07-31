using Cysharp.Threading.Tasks;
using Helpers;
using Helpers.Services;

namespace Source.Boot
{
public class StateGameplay : StateBase
{
    IGameplayContext _gameplayContext;
    
#region Public methods

    public StateGameplay(ISceneLoaderService sceneLoaderService,
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
        await LoadGameplayScene();
        SetMusicState(EMusicStates.Gameplay);

        await HideSplashScreen();

        _gameplayContext.PlayerFacade.Init();
        await _gameplayContext.PlayerFacade.ShowPlayer();
    }

    public override async UniTask Exit()
    {
        await UnloadScene(ConstSceneNames.GAME_SCENE);
    }

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

        await UniTask.WhenAll(_gameplayContext.BankLoader.Init(),
                              _menuContext.UIMenuFacade.Init(UpdateProgress));
        await UniTask.WhenAll(_gameplayMediator.Init(_canvasInputHandler.gameObject));

        sceneLoadProgress.SetupProgress = 1;
    }

    protected override async UniTask DeInitSceneContext()
    {
        _menuContext.UIMenuFacade.Deinit();
        _menuContext.BankLoader.Deinit();
        _menuContext.PlayerFacade.Deinit();

        await UniTask.CompletedTask;
    }

#endregion
}
}