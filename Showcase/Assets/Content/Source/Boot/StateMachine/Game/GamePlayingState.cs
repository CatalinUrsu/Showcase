using R3;
using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GamePlayingState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly IInputManager _inputManager;
    readonly IGameCommands _gameCommands;
    readonly IGameRunModelController _gameRunModelController;
    readonly EntryPointRegistry _entryPointRegistry;
    readonly CompositeDisposable _subscriptions = new();

#endregion

#region Public methods

    public GamePlayingState(IGameCommands gameCommands,
                            IGameRunModelController gameRunModelController,
                            EntryPointRegistry entryPointRegistry,
                            IInputManager inputManager)
    {
        _inputManager = inputManager;
        _gameCommands = gameCommands;
        _gameRunModelController = gameRunModelController;
        _entryPointRegistry = entryPointRegistry;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameEntryPoint = _entryPointRegistry.Get<IGameplayEntryPoint>();

        // After start the panel is already Game (no-op), after pause it switches back
        using (_inputManager.LockInputSystem())
            await gameEntryPoint.UIFacade.SelectPanel(EGamePanels.Game);

        _gameCommands.Stream
                     .Where(command => command == EGameCommand.Pause && _gameRunModelController.CanPause)
                     .Subscribe(_ => StatesMachine.Enter<GamePauseState>())
                     .AddTo(_subscriptions);

        // Emits current value on subscribe, so death during panel switch is not missed
        _gameRunModelController.IModel.PlayerIsKilledRef
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
