using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GamePauseState : IStateEnter
{
    public StatesMachine StatesMachine { get; set; }

    readonly IPauseService _pauseService;
    readonly IGameRunModelController _gameRunModelController;
    readonly EntryPointRegistry _entryPointRegistry;

    public GamePauseState(IPauseService pauseService,
                          IGameRunModelController gameRunModelController,
                          EntryPointRegistry entryPointRegistry)
    {
        _pauseService = pauseService;
        _gameRunModelController = gameRunModelController;
        _entryPointRegistry = entryPointRegistry;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameplay = _entryPointRegistry.Get<IGameplayEntryPoint>();

        _pauseService.Pause(EVolumeState.Pause, freezeTime: true);
        _gameRunModelController.OnContinueGame += Continue;
        _gameRunModelController.OnClickGoHome += GoHome;

        using (InputManager.Instance.LockInputSystem())
            await gameplay.UIFacade.SelectPanel(EGamePanels.Pause);
    }

    // Pause is removed here, whatever state is next
    public UniTask Exit()
    {
        _gameRunModelController.OnContinueGame -= Continue;
        _gameRunModelController.OnClickGoHome -= GoHome;
        _pauseService.Resume();
        return UniTask.CompletedTask;
    }

    void Continue() => StatesMachine.Enter<GamePlayingState>();

    void GoHome() => StatesMachine.Enter<GameUnloadState>();
}
}
