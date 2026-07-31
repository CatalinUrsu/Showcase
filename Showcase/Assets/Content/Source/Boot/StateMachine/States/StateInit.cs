using UnityEngine;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class StateInit : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly ISceneLoaderService _sceneLoaderService;
    readonly ISplashScreen _splashScreen;
    readonly IAudioService _audioService;
    readonly ISettingsModel _settingsModel;

#endregion

#region Public methods

    public StateInit(ISceneLoaderService sceneLoaderService,
                     ISplashScreen splashScreen,
                     IAudioService audioService,
                     ISettingsModelController settingsModelController)
    {
        _sceneLoaderService = sceneLoaderService;
        _splashScreen = splashScreen;
        _audioService = audioService;
        _settingsModel = settingsModelController.IModel;
    }

    public async UniTask Enter()
    {
        InitAudioService();
        await LoadAndShowSplashScreen();
        await StatesMachine.Enter<StateMenu>();
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
        await _splashScreen.Show(true);
    }

    async UniTask UnloadInitScene()
    {
        await _sceneLoaderService.UnloadScene(ConstSceneNames.INIT_SCENE);
        await Resources.UnloadUnusedAssets();
    }

#endregion
}
}