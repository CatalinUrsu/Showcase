using R3;
using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GamePauseState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly IInputManager _inputManager;
    readonly IPauseService _pauseService;
    readonly IGameCommands _gameCommands;
    readonly EntryPointRegistry _entryPointRegistry;
    readonly CompositeDisposable _subscriptions = new();

#endregion

#region Public methods

    public GamePauseState(IPauseService pauseService,
                          IGameCommands gameCommands,
                          EntryPointRegistry entryPointRegistry,
                          IInputManager inputManager)
    {
        _inputManager = inputManager;
        _pauseService = pauseService;
        _gameCommands = gameCommands;
        _entryPointRegistry = entryPointRegistry;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameEntryPoint = _entryPointRegistry.Get<IGameplayEntryPoint>();

        _pauseService.Pause(EVolumeState.Pause, freezeTime: true);
        _gameCommands.Stream.Subscribe(OnGameCommand).AddTo(_subscriptions);

        using (_inputManager.LockInputSystem())
            await gameEntryPoint.UIFacade.SelectPanel(EGamePanels.Pause);
    }

    // Pause is removed here, whatever state is next
    public UniTask Exit()
    {
        _subscriptions.Clear();
        _pauseService.Resume();
        return UniTask.CompletedTask;
    }

#endregion

#region Private methods

    void OnGameCommand(EGameCommand command)
    {
        switch (command)
        {
            case EGameCommand.Continue: StatesMachine.Enter<GamePlayingState>(); break;
            case EGameCommand.GoHome:   StatesMachine.Enter<GameUnloadState>();  break;
        }
    }

#endregion
}
}
