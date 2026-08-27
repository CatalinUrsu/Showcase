using Helpers;
using Helpers.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class MenuState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }
    
    readonly IAudioService _audioService;
    readonly ISceneLifecycleService _sceneLifecycleService;
    readonly ICinemachineService _cinemachineService;
    readonly ILoadingContext _loadingContext;
    readonly IMenuContext _menuContext;

#endregion

#region Public methods

    public MenuState(IAudioService audioService,
                     ISceneLifecycleService sceneLifecycleService,
                     ICinemachineService cinemachineService,
                     ILoadingContext loadingContext,
                     IMenuContext menuContext)
    {
        _audioService = audioService;
        _sceneLifecycleService = sceneLifecycleService;
        _cinemachineService = cinemachineService;
        _loadingContext = loadingContext;
        _menuContext = menuContext;
    }

    public async UniTask Enter()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            _audioService.MusicInstance.SetParameter(ConstFMOD.MUSIC_STATE, nameof(EMusicStates.Idle));

            await LoadMenuScene();
            
            _cinemachineService.ChangeState(nameof(ECinemachineState.Menu));
            await HideSplashScreen();
            await _menuContext.PlayerFacade.ShowPlayer();

            _menuContext.UIFacade.OnClickStartGame += GoToGameplay;
        }
    }

    public async UniTask Exit()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            _menuContext.UIFacade.OnClickStartGame -= GoToGameplay;

            await ShowSplashScreen();
            await UnloadMenuScene();
        }
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

        await _sceneLifecycleService.LoadAndInitScene(sceneLoadParams, _menuContext);
    }

    async UniTask UnloadMenuScene() => await _sceneLifecycleService.DeinitAndUnloadScene(ConstSceneNames.MENU_SCENE, _menuContext);

    async UniTask ShowSplashScreen() => await _loadingContext.SplashScreen.Show();

    async UniTask HideSplashScreen() => await _loadingContext.SplashScreen.Hide();

    void GoToGameplay() => StatesMachine.Enter<GameState>().GetAwaiter();

#endregion
}
}