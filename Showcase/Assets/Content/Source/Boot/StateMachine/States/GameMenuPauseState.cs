using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameMenuPauseState : GameMenuStateBase
{
    public GameMenuPauseState(ISceneLoaderService sceneLoaderService,
                              IProgressTrackingService progressTrackingService,
                              ISplashScreen splashScreen,
                              IAudioService audioService,
                              ISessionService sessionService,
                              IGameplayContext gameplayContext)
        : base(sceneLoaderService,
               progressTrackingService,
               splashScreen,
               audioService,
               sessionService,
               gameplayContext) { }

    public override async UniTask Enter()
    {
        base.Enter().Forget();
        await _gameplayContext.UIFacade.SelectPanel(EGamePanels.Pause);
    }
}
}