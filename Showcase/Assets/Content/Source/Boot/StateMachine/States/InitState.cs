using UnityEngine;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class InitState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly ISceneLoaderService _sceneLoaderService;
    readonly ILoadingContext _loadingContext;
    readonly IAudioService _audioService;
    readonly ISettingsModel _settingsModel;

#endregion

#region Public methods

    public InitState(ISceneLoaderService sceneLoaderService,
                     ILoadingContext loadingContext,
                     IAudioService audioService,
                     ISettingsModelController settingsModelController)
    {
        _sceneLoaderService = sceneLoaderService;
        _loadingContext = loadingContext;
        _audioService = audioService;
        _settingsModel = settingsModelController.IModel;
    }

    public async UniTask Enter()
    {
        InitAudioService();
        await LoadAndShowSplashScreen();
        await StatesMachine.Enter<MenuState>();
    }

    public async UniTask Exit() => await UnloadInitScene();

#endregion

#region Private methods

    void InitAudioService()
    {
        _audioService.Init();
        _audioService.SetSoundVolume(_settingsModel.SoundVolumeRef.CurrentValue);
        _audioService.SetMusicVolume(_settingsModel.MusicVolumeRef.CurrentValue);
    }

    async UniTask LoadAndShowSplashScreen()
    {
        var splashScreenLoadParams = new SceneLoadParams.Builder(ConstSceneNames.LOADING_SCENE)
                                     .SetIsAddressable(true)
                                     .Build();

        await _sceneLoaderService.LoadScene(splashScreenLoadParams, new SceneLoadResult());
        await _loadingContext.SplashScreen.Show(true);
    }

    async UniTask UnloadInitScene()
    {
        await _sceneLoaderService.UnloadScene(ConstSceneNames.INIT_SCENE);
        await Resources.UnloadUnusedAssets();
    }

#endregion
}
}