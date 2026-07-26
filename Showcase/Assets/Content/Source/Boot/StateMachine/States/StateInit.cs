using Cysharp.Threading.Tasks;
using Helpers.Services;
using UnityEngine;
using Zenject;

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
    readonly DiContainer _projectContainer;

#endregion

#region Public methods

    public StateInit(ISceneLoaderService sceneLoaderService,
                     ISplashScreen splashScreen,
                     IAudioService audioService,
                     ISettingsModelController settingsModelController,
                     DiContainer projectContainer)
    {
        _sceneLoaderService = sceneLoaderService;
        _splashScreen = splashScreen;
        _audioService = audioService;
        _settingsModel = settingsModelController.IModel;
        _projectContainer = projectContainer;
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
        _audioService.Init(_projectContainer);
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