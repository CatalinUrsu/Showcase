using UnityEngine;
using Helpers.StateMachine;
using Source.Audio;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public class StateInit : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly IServiceSceneLoader _sceneLoaderService;
    readonly IServiceSplashScreen _serviceSplashScreen;
    readonly IAudioService _audioService;

#endregion

#region Public methods

    public StateInit(IServiceSceneLoader sceneLoaderService, IServiceSplashScreen serviceSplashScreen, IAudioService audioService)
    {
        _audioService = audioService;
        _serviceSplashScreen = serviceSplashScreen;
        _sceneLoaderService = sceneLoaderService;
    }

    public async UniTaskVoid Enter()
    {
        _audioService.Init();
        await LoadAndShowSplashScreen();
        await StatesMachine.Enter<StateMenu>();
    }

    public async UniTask Exit()
    {
        await _sceneLoaderService.UnloadScene(ConstSceneNames.INIT_SCENE);
        await Resources.UnloadUnusedAssets();
    }

#endregion

#region Private methods

    async UniTask LoadAndShowSplashScreen()
    {
        var splashScreenLoadParams = new SceneLoadParams.Builder(ConstSceneNames.LOADING_SCENE)
                                     .SetTrackProgress(false)
                                     .Build();

        await _sceneLoaderService.LoadScene(splashScreenLoadParams);
        await _serviceSplashScreen.ShowPage(new SplashScreenInfo(true));
    }

#endregion
}
}