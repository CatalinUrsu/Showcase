using Helpers;
using UnityEngine;
using Helpers.Services;
using Cysharp.Threading.Tasks;

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
        await ShowSplashScreen();
        await DeInitSceneContext(ConstSceneNames.GAME_SCENE);
        UnloadScene(ConstSceneNames.GAME_SCENE);
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

        InputManager.Instance.OnToggleInputLock += OnToggleInputLock_handler;
        await UniTask.WhenAll(_gameplayContext.BankLoader.Init(),
                              _menuContext.UIMenuFacade.Init(UpdateProgress));
        
        _uiController.Init();
        _gameplayContext.PlayerFacade.Init(canvasInputHandler);
        _progressPresenter = new GameRunPresenter(_gameplayView, _gameRunModel);
        InitGameplayStateMachine();
        await _enemiesSpawner.Init();

        sceneLoadProgress.SetupProgress = 1;
    }

    protected override async UniTask DeInitSceneContext(string sceneName)
    {
        Time.timeScale = 1;
        _gameplayContext.BankLoader.Deinit();
        _gameplayContext.PlayerFacade.Deinit();
        _gameplayContext.UIMenuFacade.Deinit();
        
        _uiController.Deinit();
        _enemiesSpawner.Deinit();
        _progressPresenter.Deinit();
        _audioService.PauseSnapshot.stop(STOP_MODE.ALLOWFADEOUT);

        SessionService.Current.Save(ESaveFileType.Progress);
        InputManager.Instance.OnToggleInputLock -= OnToggleInputLock_handler;

        await UniTask.CompletedTask;
    }
    
    void GoToMenu()
    {
        GoToMenu_Async().Forget();
        return;

        async UniTaskVoid GoToMenu_Async()
        {
            using (InputManager.Instance.LockInputSystem())
                await StatesMachine.Enter<StateMenu>();
        }
    }

#endregion
}
}