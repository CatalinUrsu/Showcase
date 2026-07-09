using UnityEngine;
using Source.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public class StateInit : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly ISceneLoaderService _sceneLoaderService;
    readonly ISplashScreenService _splashScreenService;
    readonly IAudioService _audioService;

#endregion

#region Public methods

    public StateInit(ISceneLoaderService sceneLoaderService, ISplashScreenService splashScreenService, IAudioService audioService)
    {
        _audioService = audioService;
        _splashScreenService = splashScreenService;
        _sceneLoaderService = sceneLoaderService;
    }

    public async UniTask Enter()
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
                                     .SetIsAddressable(true)
                                     .Build();

        await _sceneLoaderService.LoadScene(splashScreenLoadParams);
        await _splashScreenService.Show(ConstSceneNames.LOADING_SCENE, true);
    }

#endregion
}
}