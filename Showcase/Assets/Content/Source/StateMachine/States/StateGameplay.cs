using Helpers;
using Source.Audio;
using Helpers.StateMachine;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public class StateGameplay : StateBase
{
#region Public methods

    public StateGameplay(IServiceSceneLoader sceneLoaderService, IServiceSplashScreen serviceSplashScreen, IServiceLoadingProgress loadingProgressService, IAudioService audioService)
        : base(sceneLoaderService, serviceSplashScreen, loadingProgressService, audioService) { }

    public override async UniTaskVoid Enter()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.GAME_SCENE)
                                  .SetPrompt("SceneGameplay")
                                  .SetActiveOnLoad(true)
                                  .Build();

            SetMusicState(EMusicStates.Gameplay);

            await LoadingContent(sceneLoadParams, "ContentGameplay");
            await ShowingContent();
        }
    }

    public override async UniTask Exit() => await UnloadingContent(ConstSceneNames.GAME_SCENE);

#endregion
}
}