using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameUnloadState : IStateEnter
{
    public StatesMachine StatesMachine { get; set; }

    readonly IInputLock _inputLock;
    readonly ISceneLifecycleService _sceneLifecycleService;
    readonly ISessionService _sessionService;
    readonly EntryPointRegistry _entryPointRegistry;
    readonly ILoadingEntryPoint _loadingEntryPoint;

    public GameUnloadState(ISceneLifecycleService sceneLifecycleService,
                           ISessionService sessionService,
                           EntryPointRegistry entryPointRegistry,
                           ILoadingEntryPoint loadingEntryPoint,
                           IInputLock inputLock)
    {
        _inputLock = inputLock;
        _sceneLifecycleService = sceneLifecycleService;
        _sessionService = sessionService;
        _entryPointRegistry = entryPointRegistry;
        _loadingEntryPoint = loadingEntryPoint;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameplay = _entryPointRegistry.Get<IGameplayEntryPoint>();

        using (_inputLock.LockInputSystem())
        {
            await _loadingEntryPoint.SplashScreen.Show();
            _sessionService.Save(ESaveFileType.Progress);
            await _sceneLifecycleService.DeinitAndUnloadScene(ConstSceneNames.GAME_SCENE, gameplay);
        }

        StatesMachine.Enter<MenuState>();
    }

    public UniTask Exit() => UniTask.CompletedTask;
}
}
