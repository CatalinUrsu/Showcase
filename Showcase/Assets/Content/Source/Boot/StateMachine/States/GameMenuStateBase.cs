using Helpers;
using FMOD.Studio;
using UnityEngine;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameMenuStateBase : StateBase
{
    bool _isGoingHome;
    ISessionService _sessionService;
    protected IGameplayContext _gameplayContext;

#region Public methods

    public GameMenuStateBase(ISceneLoaderService sceneLoaderService,
                             IProgressTrackingService progressTrackingService,
                             ISplashScreen splashScreen,
                             IAudioService audioService,
                             ISessionService sessionService,
                             IGameplayContext gameplayContext)
        : base(sceneLoaderService, progressTrackingService, splashScreen, audioService)
    {
        _sessionService = sessionService;
        _gameplayContext = gameplayContext;
    }

    public override async UniTask Enter()
    {
        Time.timeScale = 0;
        AddGameRunControllerListeners();
        _audioService.PauseSnapshot.start();
        await UniTask.CompletedTask;
    }

    public override async UniTask Exit()
    {
        RemoveGameRunControllerListeners();
        _audioService.PauseSnapshot.stop(STOP_MODE.ALLOWFADEOUT);

        if (_isGoingHome)
            await LeaveGameplay();
        else
            await ReturnToGame();
    }

#endregion

#region Private methods

    protected override async UniTask DeInitSceneContext(string sceneName)
    {
        _gameplayContext.BankLoader.Deinit();
        _gameplayContext.PlayerFacade.Deinit();
        _gameplayContext.UIFacade.Deinit();
        _gameplayContext.EnemiesController.Deinit();

        Time.timeScale = 1;

        await base.DeInitSceneContext(sceneName);
        _sessionService.Save(ESaveFileType.Progress);
    }

    protected virtual UniTask ReturnToGame() => UniTask.CompletedTask;

    void AddGameRunControllerListeners()
    {
        _gameplayContext.GameRunModelController.OnClickGoHome += GoToMenuState;
        _gameplayContext.GameRunModelController.OnClickReturnToGame += GoToGameState;
    }

    void RemoveGameRunControllerListeners()
    {
        _gameplayContext.GameRunModelController.OnClickGoHome -= GoToMenuState;
        _gameplayContext.GameRunModelController.OnClickReturnToGame -= GoToGameState;
    }

    async UniTask LeaveGameplay()
    {
        await ShowSplashScreen();
        await DeInitSceneContext(ConstSceneNames.GAME_SCENE);
        UnloadScene(ConstSceneNames.GAME_SCENE);
    }

    void GoToMenuState()
    {
        GoToMenu_Async().Forget();
        return;

        async UniTaskVoid GoToMenu_Async()
        {
            _isGoingHome = true;
            using (InputManager.Instance.LockInputSystem())
                await StatesMachine.Enter<MenuState>();
        }
    }

    void GoToGameState()
    {
        GoToGame_Async().Forget();
        return;

        async UniTaskVoid GoToGame_Async()
        {
            _isGoingHome = false;
            using (InputManager.Instance.LockInputSystem())
                await StatesMachine.Enter<GameplayState>();
        }
    }

#endregion
}
}