using Helpers.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class MenuEntryPoint : IMenuEntryPoint
{
#region Fields

    public IPlayerFacade PlayerFacade { get; }
    public IMenuUIFacade UIFacade { get; }

    readonly IBankLoader _bankLoader;

#endregion

#region Public methods

    public MenuEntryPoint(IBankLoader bankLoader, IPlayerFacade playerFacade, IMenuUIFacade uiFacade)
    {
        _bankLoader = bankLoader;
        PlayerFacade = playerFacade;
        UIFacade = uiFacade;
    }

    public async UniTask Init(IProgressTrackingService progressTrackingService, SceneLoadProgress sceneLoadProgress)
    {
        progressTrackingService.UpdateLoadingTip("Setup Menu Scene");

        await _bankLoader.Init();
        await UIFacade.Init(UpdateProgress);

        PlayerFacade.Init();
        return;

        void UpdateProgress(float progress)
        {
            sceneLoadProgress.SetupProgress += progress;
            progressTrackingService.UpdateProgress();
        }
    }

    public async UniTask DeInit()
    {
        _bankLoader.Deinit();
        PlayerFacade.Deinit();
        await UIFacade.Deinit();
    }

#endregion
}
}
