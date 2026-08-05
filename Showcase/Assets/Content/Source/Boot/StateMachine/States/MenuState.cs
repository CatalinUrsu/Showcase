using Helpers;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class MenuState : StateBase
{
    readonly IMenuContext _menuContext;

#region Public methods

    public MenuState(ISceneLoaderService sceneLoaderService,
                     IProgressTrackingService progressTrackingService,
                     ISplashScreen splashScreen,
                     IAudioService audioService,
                     IMenuContext menuContext)
        : base(sceneLoaderService, progressTrackingService, splashScreen, audioService)
    {
        _menuContext = menuContext;
    }

    public override async UniTask Enter()
    {
        SetMusicState(EMusicStates.Idle);
        _menuContext.UIFacade.OnClickStartGame += GoToGameplay;

        await LoadMenuScene();
        await HideSplashScreen();
        _menuContext.PlayerFacade.Init();
        await _menuContext.PlayerFacade.ShowPlayer();
    }

    public override async UniTask Exit()
    {
        if (_menuContext.UIFacade != null)
            _menuContext.UIFacade.OnClickStartGame -= GoToGameplay;

        await ShowSplashScreen();
        await DeInitSceneContext(ConstSceneNames.MENU_SCENE);
        UnloadScene(ConstSceneNames.MENU_SCENE);
    }

#endregion

#region Private methods

    async UniTask LoadMenuScene()
    {
        var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.MENU_SCENE)
                              .SetTip("Load Menu Scene")
                              .SetIsAddressable(true)
                              .SetActiveOnLoad(true)
                              .Build();
        await LoadScene(sceneLoadParams);
    }

    protected override async UniTask InitSceneContext(SceneLoadProgress sceneLoadProgress)
    {
        _progressTrackingService.UpdateLoadingTip("Setup Menu Scene");

        await UniTask.WhenAll(_menuContext.BankLoader.Init(),
                              _menuContext.UIFacade.Init(UpdateProgress));
        return;

        void UpdateProgress(float progress)
        {
            sceneLoadProgress.SetupProgress += progress;
            _progressTrackingService.UpdateProgress();
        }
    }

    protected override async UniTask DeInitSceneContext(string sceneName)
    {
        await base.DeInitSceneContext(sceneName);

        _menuContext.BankLoader.Deinit();
        _menuContext.PlayerFacade.Deinit();
        await _menuContext.UIFacade.Deinit();
    }

    void GoToGameplay()
    {
        GoToGameplay_Async().Forget();
        return;

        async UniTaskVoid GoToGameplay_Async()
        {
            using (InputManager.Instance.LockInputSystem())
                await StatesMachine.Enter<GameplayState>();
        }
    }

#endregion
}
}