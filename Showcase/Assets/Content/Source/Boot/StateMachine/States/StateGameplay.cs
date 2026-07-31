using Cysharp.Threading.Tasks;
using Helpers;
using Helpers.Services;

namespace Source.Boot
{
public class StateGameplay : StateBase
{
    IGameplayContext _gameplayContext;
    
#region Public methods

    public StateGameplay(ISceneLoaderService sceneLoaderService,
                         IProgressTrackingService progressTrackingService,
                         ISplashScreen splashScreen,
                         IAudioService audioService,
                         IGameplayContext gameplayContext)
        : base(sceneLoaderService, progressTrackingService, splashScreen, audioService)
    {
        _gameplayContext = gameplayContext;
    }

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