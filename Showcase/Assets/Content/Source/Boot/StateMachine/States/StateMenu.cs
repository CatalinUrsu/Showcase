using Helpers;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class StateMenu : StateBase
{
    readonly IMenuContext _menuContext;

#region Public methods

    public StateMenu(ISceneLoaderService sceneLoaderService,
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
        using (InputManager.Instance.LockInputSystem())
        {
            SetMusicState(EMusicStates.Idle);
            _menuContext.UIMenuFacade.OnClickStartGame += GoToGameplay;

            await LoadMenuScene();
            await HideSplashScreen();
            await _menuContext.PlayerFacade.ShowPlayer();
        }
    }

    public override async UniTask Exit()
    {
        if (_menuContext.UIMenuFacade != null)
            _menuContext.UIMenuFacade.OnClickStartGame -= GoToGameplay;

        await ShowSplashScreen();
        await DeInitSceneContext();
        await UnloadScene(ConstSceneNames.MENU_SCENE);
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
                              _menuContext.UIMenuFacade.Init(UpdateProgress));
        return;

        void UpdateProgress(float progress)
        {
            sceneLoadProgress.SetupProgress += progress;
            _progressTrackingService.UpdateProgress();
        }
    }

    protected override async UniTask DeInitSceneContext()
    {
        _menuContext.UIMenuFacade.Deinit();
        _menuContext.BankLoader.Deinit();
        _menuContext.PlayerFacade.Deinit();

        await UniTask.CompletedTask;
    }

    void GoToGameplay()
    {
        GoToGameplay_Async().Forget();
        return;

        async UniTaskVoid GoToGameplay_Async()
        {
            using (InputManager.Instance.LockInputSystem())
                await StatesMachine.Enter<StateGameplay>();
        }
    }

#endregion
}
}