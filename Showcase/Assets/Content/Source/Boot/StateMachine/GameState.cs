using Helpers;
using FMOD.Studio;
using UnityEngine;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }
    
    readonly IAudioService _audioService;
    readonly ISceneLifecycleService _sceneLifecycleService;
    readonly ISessionService _sessionService;
    readonly ICinemachineService _cinemachineService;
    readonly IVolumeSwitcherService _volumeSwitcherService;
    readonly IGameRunModelController _gameRunModelController;
    readonly ILoadingContext _loadingContext;
    readonly IGameplayContext _gameplayContext;

#endregion
    
#region Public methods

    public GameState(IAudioService audioService,
                     ISceneLifecycleService sceneLifecycleService,
                     ISessionService sessionService,
                     ICinemachineService cinemachineService,
                     IVolumeSwitcherService volumeSwitcherService,
                     IGameRunModelController gameRunModelController,
                     ILoadingContext loadingContext,
                     IGameplayContext gameplayContext)
    {
        _audioService = audioService;
        _sceneLifecycleService = sceneLifecycleService;
        _sessionService = sessionService;
        _cinemachineService = cinemachineService;
        _volumeSwitcherService = volumeSwitcherService;
        _gameRunModelController = gameRunModelController;
        _loadingContext = loadingContext;
        _gameplayContext = gameplayContext;
    }

    public async UniTask Enter()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            await LoadGameScene();
            _gameRunModelController.StartRun();
            _cinemachineService.SetCinemachineGame(_gameplayContext.PlayerFacade.Transform, _gameplayContext.GameCameraBounds);
            _cinemachineService.ChangeState(ECinemachineState.Game);
            
            await HideSplashScreen();
            await StartGameplay();
            
            AddGameRunControllerListeners();
        }
    }

    public async UniTask Exit()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            RemoveGameRunControllerListeners();
            await ShowSplashScreen();
            
            _sessionService.Save(ESaveFileType.Progress);
            _cinemachineService.ChangeState(ECinemachineState.Menu);
            _volumeSwitcherService.ChangeState(EVolumeState.Global);

            ApplyResume();
            await UnloadGameScene();
        }
    }

#endregion

#region Private methods
    
    async UniTask LoadGameScene()
    {
        var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.GAME_SCENE)
                              .SetTip("Load Gameplay Scene")
                              .SetIsAddressable(true)
                              .SetActiveOnLoad(true)
                              .Build();
        
        await _sceneLifecycleService.LoadAndInitScene(sceneLoadParams, _gameplayContext);
    }
    
    async UniTask UnloadGameScene() => await _sceneLifecycleService.DeinitAndUnloadScene(ConstSceneNames.GAME_SCENE, _gameplayContext);

    async UniTask StartGameplay()
    {
        _gameplayContext.EnemiesSpawner.ToggleSpawning(true);

        await UniTask.WhenAll(_gameplayContext.UIFacade.SelectPanel(EGamePanels.Game),
                              _gameplayContext.PlayerFacade.ShowPlayer());

        _gameplayContext.PlayerFacade.ToggleControl(true);
    }

    async UniTask ShowSplashScreen() => await _loadingContext.SplashScreen.Show();

    async UniTask HideSplashScreen() => await _loadingContext.SplashScreen.Hide();

    void AddGameRunControllerListeners()
    {
        _gameRunModelController.OnClickPause += OpenPause;
        _gameRunModelController.OnPlayerLoose += OpenLooseMenu;
        _gameRunModelController.OnClickGoHome += GoToMenu;
        _gameRunModelController.OnContinueGame += ContinueGame;
        _gameRunModelController.OnRestartRun += RestartGame;
    }

    void RemoveGameRunControllerListeners()
    {
        _gameRunModelController.OnClickPause -= OpenPause;
        _gameRunModelController.OnPlayerLoose -= OpenLooseMenu;
        _gameRunModelController.OnClickGoHome -= GoToMenu;
        _gameRunModelController.OnContinueGame -= ContinueGame;
        _gameRunModelController.OnRestartRun -= RestartGame;
    }

    void OpenPause()
    {
        Time.timeScale = 0;
        ApplyPause();
        SelectUIPanel(EGamePanels.Pause).Forget();
        _volumeSwitcherService.ChangeState(EVolumeState.Pause);
    }

    void OpenLooseMenu()
    {
        ApplyPause();
        SelectUIPanel(EGamePanels.Loose).Forget();
        _volumeSwitcherService.ChangeState(EVolumeState.Loose);
    }
    
    void ContinueGame()
    {
        ApplyResume();
        SelectUIPanel(EGamePanels.Game).Forget();
        _volumeSwitcherService.ChangeState(EVolumeState.Global);
    }
    
    void RestartGame()
    {
        ApplyResume();
        _volumeSwitcherService.ChangeState(EVolumeState.Global);
        _gameRunModelController.StartRun();
        RestartGameLogic().Forget();
        return;
        
        async UniTaskVoid RestartGameLogic()
        {
            await UniTask.WhenAll(SelectUIPanel(EGamePanels.Game),
                                  _gameplayContext.PlayerFacade.ShowPlayer());

            _gameplayContext.PlayerFacade.ToggleControl(true);
        }
    }

    void ApplyPause() => _audioService.PauseSnapshot.start();

    void ApplyResume()
    {
        Time.timeScale = 1;
        _audioService.PauseSnapshot.stop(STOP_MODE.ALLOWFADEOUT);
    }

    async UniTask SelectUIPanel(EGamePanels panelType)
    {
        using (InputManager.Instance.LockInputSystem())
            await _gameplayContext.UIFacade.SelectPanel(panelType);
    }

    void GoToMenu() => StatesMachine.Enter<MenuState>().GetAwaiter();

#endregion
}
}