using R3;
using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameLooseState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly IPauseService _pauseService;
    readonly ICinemachineService _cinemachineService;
    readonly IGameCommands _gameCommands;
    readonly EntryPointRegistry _entryPointRegistry;
    readonly CompositeDisposable _subscriptions = new();

#endregion

#region Public methods

    public GameLooseState(IPauseService pauseService,
                          ICinemachineService cinemachineService,
                          IGameCommands gameCommands,
                          EntryPointRegistry entryPointRegistry)
    {
        _pauseService = pauseService;
        _cinemachineService = cinemachineService;
        _gameCommands = gameCommands;
        _entryPointRegistry = entryPointRegistry;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameEntryPoint = _entryPointRegistry.Get<IGameplayEntryPoint>();

        _pauseService.Pause(EVolumeState.Loose, freezeTime: false);
        _cinemachineService.SetFollow(ECinemachineState.Game, null);
        _gameCommands.Stream.Subscribe(OnGameCommand).AddTo(_subscriptions);

        using (InputManager.Instance.LockInputSystem())
            await gameEntryPoint.UIFacade.SelectPanel(EGamePanels.Loose);
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
            case EGameCommand.Restart: StatesMachine.Enter<GameStartState>(); break;
            case EGameCommand.GoHome:  StatesMachine.Enter<GameUnloadState>(); break;
        }
    }

#endregion
}
}
