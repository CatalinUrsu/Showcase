using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameLooseState : IStateEnter
{
    public StatesMachine StatesMachine { get; set; }

    readonly IPauseService _pauseService;
    readonly ICinemachineService _cinemachineService;
    readonly IGameRunModelController _gameRunModelController;
    readonly EntryPointRegistry _entryPointRegistry;

    public GameLooseState(IPauseService pauseService,
                          ICinemachineService cinemachineService,
                          IGameRunModelController gameRunModelController,
                          EntryPointRegistry entryPointRegistry)
    {
        _pauseService = pauseService;
        _cinemachineService = cinemachineService;
        _gameRunModelController = gameRunModelController;
        _entryPointRegistry = entryPointRegistry;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameplay = _entryPointRegistry.Get<IGameplayEntryPoint>();

        _pauseService.Pause(EVolumeState.Loose, freezeTime: false);
        _cinemachineService.SetFollow(ECinemachineState.Game, null);
        _gameRunModelController.OnRestartRun += Restart;
        _gameRunModelController.OnClickGoHome += GoHome;

        using (InputManager.Instance.LockInputSystem())
            await gameplay.UIFacade.SelectPanel(EGamePanels.Loose);
    }

    // Pause is removed here, whatever state is next
    public UniTask Exit()
    {
        _gameRunModelController.OnRestartRun -= Restart;
        _gameRunModelController.OnClickGoHome -= GoHome;
        _pauseService.Resume();
        return UniTask.CompletedTask;
    }

    void Restart() => StatesMachine.Enter<GameStartState>();

    void GoHome() => StatesMachine.Enter<GameUnloadState>();
}
}
