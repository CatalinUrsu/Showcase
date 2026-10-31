using Source.Data;
using UnityEngine;
using Helpers.Services;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class InitState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly ISceneLoaderService _sceneLoaderService;
    readonly ILoadingEntryPoint _loadingEntryPoint;
    readonly IAudioService _audioService;
    readonly SettingsModel _settingsModel;
    readonly ILocaleService _localeService;

#endregion

#region Public methods

    public InitState(ISceneLoaderService sceneLoaderService,
                     ILoadingEntryPoint loadingEntryPoint,
                     IAudioService audioService,
                     SettingsModel settings,
                     ILocaleService localeService)
    {
        _sceneLoaderService = sceneLoaderService;
        _loadingEntryPoint = loadingEntryPoint;
        _audioService = audioService;
        _settingsModel = settings;
        _localeService = localeService;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        InitAudioService();
        await ApplySavedLocale();
        await LoadAndShowSplashScreen();
        StatesMachine.Enter<MenuState>();
    }

    public async UniTask Exit() => await UnloadInitScene();

#endregion

#region Private methods

    void InitAudioService()
    {
        _audioService.Init();
        _audioService.SetSoundVolume(_settingsModel.SoundVolume.CurrentValue);
        _audioService.SetMusicVolume(_settingsModel.MusicVolume.CurrentValue);
    }

    async UniTask ApplySavedLocale()
    {
        await _localeService.Init();
        _localeService.SetLocale(_settingsModel.LocaleIdx.CurrentValue);
    }

    async UniTask LoadAndShowSplashScreen()
    {
        var splashScreenLoadParams = new SceneLoadParams.Builder(ConstSceneNames.LOADING_SCENE)
                                     .SetIsAddressable(true)
                                     .Build();

        await _sceneLoaderService.LoadScene(splashScreenLoadParams, new SceneLoadResult());
        await _loadingEntryPoint.SplashScreen.Show(true);
    }

    async UniTask UnloadInitScene()
    {
        await _sceneLoaderService.UnloadScene(ConstSceneNames.INIT_SCENE);
        await Resources.UnloadUnusedAssets();
    }

#endregion
}
}