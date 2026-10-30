using Helpers;
using Helpers.Audio;
using Helpers.Services;
using System.Threading;
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
    readonly ILoadingEntryPoint _loadingEntryPoint;
    
    IMenuEntryPoint _menuEntryPoint;

#endregion

#region Public methods

    public MenuState(IAudioService audioService,
                     ISceneLifecycleService sceneLifecycleService,
                     ICinemachineService cinemachineService,
                     ILoadingEntryPoint loadingEntryPoint)
    {
        _audioService = audioService;
        _sceneLifecycleService = sceneLifecycleService;
        _cinemachineService = cinemachineService;
        _loadingEntryPoint = loadingEntryPoint;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        using (InputManager.Instance.LockInputSystem())
        {
            _audioService.MusicInstance.SetParameter(ConstFMOD.MUSIC_STATE, nameof(EMusicStates.Idle));

            await LoadMenuScene();
            
            _cinemachineService.ChangeState(nameof(ECinemachineState.Menu));
            await HideSplashScreen();
            await _menuEntryPoint.PlayerFacade
                                 .ShowPlayer()
                                 .AttachExternalCancellation(cancellationToken);

            _menuEntryPoint.UIFacade.OnClickStartGame += GoToGameplay;
        }
    }

    public async UniTask Exit()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            _menuEntryPoint.UIFacade.OnClickStartGame -= GoToGameplay;

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

        _menuEntryPoint = await _sceneLifecycleService.LoadAndInitScene<IMenuEntryPoint>(sceneLoadParams);
    }

    async UniTask UnloadMenuScene()
    {
        await _sceneLifecycleService.DeinitAndUnloadScene(ConstSceneNames.MENU_SCENE, _menuEntryPoint);
        _menuEntryPoint = null;
    }

    async UniTask ShowSplashScreen() => await _loadingEntryPoint.SplashScreen.Show();

    async UniTask HideSplashScreen() => await _loadingEntryPoint.SplashScreen.Hide();

    void GoToGameplay() => StatesMachine.Enter<GameLoadState>();

#endregion
}
}