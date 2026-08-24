using Helpers.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class MenuContext : IMenuContext
{
#region Fields

    public IBankLoader BankLoader { get; private set; }
    public IPlayerFacade PlayerFacade { get; private set; }
    public IMenuUIFacade UIFacade { get; private set; }

#endregion

#region Fields Registration

    public void RegisterBankLoader(IBankLoader bankLoader) => BankLoader = bankLoader;

    public void RegisterPlayerFacade(IPlayerFacade playerFacade) => PlayerFacade = playerFacade;

    public void RegisterUIMenuFacade(IMenuUIFacade menuUIFacade) => UIFacade = menuUIFacade;

#endregion

#region Public methods

    public async UniTask Init(IProgressTrackingService progressTrackingService, SceneLoadProgress sceneLoadProgress)
    {
        progressTrackingService.UpdateLoadingTip("Setup Menu Scene");

        await BankLoader.Init();
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
        BankLoader.Deinit();
        PlayerFacade.Deinit();
        await UIFacade.Deinit();
        
        Clear();
    }
    
    void Clear()
    {
        BankLoader = null;
        PlayerFacade = null;
        UIFacade = null;
    }

#endregion
}
}