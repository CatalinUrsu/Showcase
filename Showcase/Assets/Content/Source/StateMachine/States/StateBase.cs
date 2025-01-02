using Helpers;
using UnityEngine;
using Helpers.StateMachine;
using Source.Audio;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public abstract class StateBase : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    protected readonly IServiceSceneLoader _sceneLoaderService;
    protected readonly IServiceSplashScreen _serviceSplashScreen;
    protected readonly IServiceLoadingProgress _loadingProgressService;
    protected readonly IAudioService _audioService;

    IEntryPoint _entryPoint;

#endregion

#region Public methods

    public StateBase(IServiceSceneLoader sceneLoaderService, IServiceSplashScreen serviceSplashScreen, IServiceLoadingProgress loadingProgressService, IAudioService audioService)
    {
        _audioService = audioService;
        _loadingProgressService = loadingProgressService;
        _serviceSplashScreen = serviceSplashScreen;
        _sceneLoaderService = sceneLoaderService;
    }

    public abstract UniTaskVoid Enter();

    public abstract UniTask Exit();

#endregion

#region Protected & Private methods

    protected void SetMusicState(EMusicStates state) => _audioService.MusicInstance.SetParameter(ConstFMOD.MUSIC_STATE, state.ToString());

    protected async UniTask LoadingContent(SceneLoadParams sceneLoadParams, string InitContentPrompt)
    {
        _loadingProgressService.LoadProgressCount = 1;
        var sceneResult = await _sceneLoaderService.LoadScene(sceneLoadParams);
        _entryPoint = sceneResult.LoadedScene.FindEntryPoint();

        //Simultaneously wait content unloading and load another (ex: unload menu scene, init EntryPoint)
        await UniTask.WhenAll(InitEntyPoint(sceneResult.SceneLoadProgress, InitContentPrompt),
                              UniTask.WaitUntil(() => _loadingProgressService.UnloadsAreFinished)
                                     .ContinueWith(() => Resources.UnloadUnusedAssets().ToUniTask()));
    }

    protected async UniTask ShowingContent()
    {
        await UniTask.WhenAll(_serviceSplashScreen.HidePage(),
                              _entryPoint.Enter());
    }

    protected async UniTask UnloadingContent(string sceneName)
    {
        await _serviceSplashScreen.ShowPage(new SplashScreenInfo());

        FmodExtensions.ReleaseInstanceByScene(sceneName);
        var unloadingTasks = _entryPoint.Exit().ContinueWith(() => _sceneLoaderService.UnloadScene(sceneName));
        _loadingProgressService.RegisterUnloadingTasks(unloadingTasks).Forget();
    }

    async UniTask InitEntyPoint(SceneLoadProgress sceneLoadProgress, string InitContentPrompt)
    {
        _loadingProgressService.UpdateLoadingPrompt(InitContentPrompt);

        await _entryPoint.Init(StatesMachine, UpdateInitProgress);

        void UpdateInitProgress(float progress)
        {
            sceneLoadProgress.EntryPointProgress += progress;
            _loadingProgressService.UpdateProgress();
        }
    }

#endregion
}
}