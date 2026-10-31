using R3;
using Helpers;
using Source.Data;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GamePlayingState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly EntryPointRegistry _entryPointRegistry;
    readonly GameRunModel _gameRun;
    readonly IGameCommands _gameCommands;
    readonly IInputManager _inputManager;
    readonly CompositeDisposable _subscriptions = new();

#endregion

#region Public methods

    public GamePlayingState(EntryPointRegistry entryPointRegistry,
                            GameRunModel gameRun,
                            IGameCommands gameCommands,
                            IInputManager inputManager)
    {
        _entryPointRegistry = entryPointRegistry;
        _gameRun = gameRun;
        _gameCommands = gameCommands;
        _inputManager = inputManager;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameEntryPoint = _entryPointRegistry.Get<IGameplayEntryPoint>();

        // After start the panel is already Game (no-op), after pause it switches back
        using (_inputManager.LockInputSystem())
            await gameEntryPoint.UIFacade.SelectPanel(EGamePanels.Game);

        _gameCommands.Stream
                     .Where(command => command == EGameCommand.Pause && _gameRun.CanPause)
                     .Subscribe(_ => StatesMachine.Enter<GamePauseState>())
                     .AddTo(_subscriptions);

        // Emits current value on subscribe, so death during panel switch is not missed
        _gameRun.PlayerIsKilled
                               .Where(isKilled => isKilled)
                               .Subscribe(_ => StatesMachine.Enter<GameLooseState>())
                               .AddTo(_subscriptions);
    }

    public UniTask Exit()
    {
        _subscriptions.Clear();
        return UniTask.CompletedTask;
    }

#endregion
}
}
