using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameMenuLooseState : GameMenuStateBase
{
    public GameMenuLooseState(ISceneLoaderService sceneLoaderService,
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
        await _gameplayContext.UIFacade.ShowPanel(EGamePanels.Loose);
    }

    protected override async UniTask ReturnToGame()
    {
        await _gameplayContext.UIFacade.HidePanel(EGamePanels.Loose);
        //TODO: Spawn and init new player
    }
}
}