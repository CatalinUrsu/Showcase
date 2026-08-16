using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameMenuPauseState : GameMenuStateBase
{
    public GameMenuPauseState(ISceneLoaderService sceneLoaderService,
                              IProgressTrackingService progressTrackingService,
                              IAudioService audioService,
                              ISessionService sessionService,
                              ILoadingContext loadingContext,
                              IGameplayContext gameplayContext)
        : base(sceneLoaderService,
               progressTrackingService,
               audioService,
               sessionService,
               loadingContext,
               gameplayContext) { }

    public override async UniTask Enter()
    {
        base.Enter().Forget();
        await _gameplayContext.UIFacade.SelectPanel(EGamePanels.Pause);
    }
}
}