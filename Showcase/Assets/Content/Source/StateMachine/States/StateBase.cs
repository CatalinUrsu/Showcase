using UnityEngine;
using Source.Audio;
using Helpers.Audio;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public abstract class StateBase : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly ISceneLoaderService _sceneLoaderService;
    readonly ISplashScreenService _splashScreenService;
    readonly IProgressTrackingService progressTrackingService;
    readonly IAudioService _audioService;

#endregion

#region Public methods

    public StateBase(ISceneLoaderService sceneLoaderService, ISplashScreenService splashScreenService, IProgressTrackingService progressTrackingService, IAudioService audioService)
    {
        _audioService = audioService;
        this.progressTrackingService = progressTrackingService;
        _splashScreenService = splashScreenService;
        _sceneLoaderService = sceneLoaderService;
    }

    public abstract UniTask Enter();

    public abstract UniTask Exit();

#endregion

#region Protected & Private methods

    protected void SetMusicState(EMusicStates state) => _audioService.MusicInstance.SetParameter(ConstFMOD.MUSIC_STATE, state.ToString());

    protected async UniTask LoadingContent(SceneLoadParams sceneLoadParams, string initContentPrompt)
    {
        progressTrackingService.LoadProgressCount = 1;
        var sceneResult = await _sceneLoaderService.LoadScene(sceneLoadParams);
        // _entryPoint = sceneResult.LoadedScene.FindEntryPoint();

        //Simultaneously wait content unloading and load another (ex: unload menu scene, init EntryPoint)
        await UniTask.WhenAll(InitEntryPoint(sceneResult.SceneLoadProgress, initContentPrompt),
                              UniTask.WaitUntil(() => progressTrackingService.UnloadsAreFinished)
                                     .ContinueWith(() => Resources.UnloadUnusedAssets().ToUniTask()));
    }

    protected async UniTask ShowingContent()
    {
        // await UniTask.WhenAll(_serviceSplashScreen.HidePage(),
        //                       _entryPoint.Enter());
    }

    protected async UniTask UnloadingContent(string sceneName)
    {
        await _splashScreenService.Show(ConstSceneNames.LOADING_SCENE);

        FmodExtensions.ReleaseSceneInstances(sceneName);
        // var unloadingTasks = _entryPoint.Exit().ContinueWith(() => _sceneLoaderService.UnloadScene(sceneName));
        // _loadingProgressService.RegisterUnloadProcesses(unloadingTasks);
    }

    async UniTask InitEntryPoint(SceneLoadProgress sceneLoadProgress, string initContentPrompt)
    {
        progressTrackingService.UpdateLoadingTip(initContentPrompt);

        // await _entryPoint.Init(StatesMachine, UpdateInitProgress);

        void UpdateInitProgress(float progress)
        {
            sceneLoadProgress.EntryPointProgress += progress;
            progressTrackingService.UpdateProgress();
        }
    }

#endregion
}
}