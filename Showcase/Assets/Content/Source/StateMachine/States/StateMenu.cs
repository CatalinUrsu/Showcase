using Helpers;
using StateMachine;
using Source.Audio;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public class StateMenu : StateBase
{
#region Public methods
    
    public StateMenu(IServiceSceneLoader sceneLoaderService, IServiceSplashScreen serviceSplashScreen, IServiceLoadingProgress loadingProgressService, IAudioService audioService)
        : base(sceneLoaderService, serviceSplashScreen, loadingProgressService, audioService) { }

    public override async UniTaskVoid Enter()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.MENU_SCENE)
                                  .SetPrompt("SceneMenu")
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