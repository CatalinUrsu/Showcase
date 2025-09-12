using Helpers;
using Source.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public class StateMenu : StateBase
{
#region Public methods
    
    public StateMenu(IServiceSceneLoader sceneLoaderService, IServiceSplashScreen serviceSplashScreen, IServiceProgressTracking loadingProgressService, IAudioService audioService)
        : base(sceneLoaderService, serviceSplashScreen, loadingProgressService, audioService) { }

    public override async UniTask Enter()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.MENU_SCENE)
                                  .SetTip("SceneMenu")
                                  .SetIsAddressable(true)
                                  .SetActiveOnLoad(true)
                                  .Build();

            SetMusicState(EMusicStates.Idle);

            await LoadingContent(sceneLoadParams, "ContentMenu");
            await ShowingContent();
        }
    }

    public override async UniTask Exit() => await UnloadingContent(ConstSceneNames.MENU_SCENE);

#endregion
}
}