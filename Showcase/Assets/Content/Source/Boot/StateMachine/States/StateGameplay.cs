using Cysharp.Threading.Tasks;
using Helpers;
using Helpers.Services;

namespace Source.Boot
{
public class StateGameplay : StateBase
{
#region Public methods

    public StateGameplay(ISceneLoaderService sceneLoaderService, ISplashScreen splashScreen, IProgressTrackingService progressTrackingService, IAudioService audioService)
        : base(sceneLoaderService, splashScreen, progressTrackingService, audioService) { }

    public override async UniTask Enter()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.GAME_SCENE)
                                  .SetTip("SceneGameplay")
                                  .SetIsAddressable(true)
                                  .SetActiveOnLoad(true)
                                  .Build();

            SetMusicState(EMusicStates.Gameplay);

            await LoadScene(sceneLoadParams, "ContentGameplay");
            await HideSplashScreen();
        }
    }

    public override async UniTask Exit() => await UnloadScene(ConstSceneNames.GAME_SCENE);

#endregion
}
}