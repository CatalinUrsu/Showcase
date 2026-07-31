using Cysharp.Threading.Tasks;
using Helpers.Audio;
using Helpers.Services;
using UnityEngine;

namespace Source.Boot
{
public abstract class StateBase : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly ISceneLoaderService _sceneLoaderService;
    protected readonly IProgressTrackingService _progressTrackingService;
    readonly ISplashScreen _splashScreen;
    readonly IAudioService _audioService;

#endregion

#region Public methods

    public StateBase(ISceneLoaderService sceneLoaderService,
                     IProgressTrackingService progressTrackingService,
                     ISplashScreen splashScreen,
                     IAudioService audioService)
    {
        _audioService = audioService;
        _progressTrackingService = progressTrackingService;
        _splashScreen = splashScreen;
        _sceneLoaderService = sceneLoaderService;
    }

    public abstract UniTask Enter();

    public abstract UniTask Exit();

#endregion

#region Protected & Private methods

    protected void SetMusicState(EMusicStates state) => _audioService.MusicInstance.SetParameter(ConstFMOD.MUSIC_STATE, state.ToString());

    protected async UniTask ShowSplashScreen() => await _splashScreen.Show();

    protected async UniTask HideSplashScreen() => await _splashScreen.Hide();

    protected async UniTask LoadScene(SceneLoadParams sceneLoadParams)
    {
        var sceneLoadResult = new SceneLoadResult();
        _progressTrackingService.RegisterLoadingProgress(sceneLoadResult.SceneLoadProgress);

        var waitUnloadingTask = UniTask.WaitUntil(() => _progressTrackingService.UnloadsFinished)
                                       .ContinueWith(() => Resources.UnloadUnusedAssets().ToUniTask());
        var loadSceneTask = _sceneLoaderService.LoadScene(sceneLoadParams, sceneLoadResult)
                                               .ContinueWith(() => InitSceneContext(sceneLoadResult.SceneLoadProgress));

        //Simultaneously wait content unloading and load another (ex: unload menu scene, init EntryPoint)
        await UniTask.WhenAll(loadSceneTask, waitUnloadingTask);

        sceneLoadResult.SceneLoadProgress.SetupProgress = 1;
    }

    protected void UnloadScene(string sceneName)
    {
        var unloadSceneTask = _sceneLoaderService.UnloadScene(sceneName);
        _progressTrackingService.RegisterUnloadProcesses(unloadSceneTask);
    }

    protected virtual async UniTask InitSceneContext(SceneLoadProgress sceneLoadProgress) { }

    protected virtual async UniTask DeInitSceneContext(string sceneName) => FmodExtensions.ReleaseSceneInstances(sceneName);

#endregion
}
}