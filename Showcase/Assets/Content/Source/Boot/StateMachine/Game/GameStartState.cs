using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;
using Source.Data;

namespace Source.Boot
{
/// <summary>
/// Starts new run: resets run model and spawns player. Used for first start and for restart.
/// </summary>
public class GameStartState : IStateEnter
{
    public StatesMachine StatesMachine { get; set; }

    readonly EntryPointRegistry _entryPointRegistry;
    readonly GameRunModel _gameRun;
    readonly IInputManager _inputManager;
    readonly ICinemachineService _cinemachineService;

    public GameStartState(EntryPointRegistry entryPointRegistry,
                          GameRunModel gameRun,
                          IInputManager inputManager,
                          ICinemachineService cinemachineService)
    {
        _entryPointRegistry = entryPointRegistry;
        _gameRun = gameRun;
        _inputManager = inputManager;
        _cinemachineService = cinemachineService;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameplay = _entryPointRegistry.Get<IGameplayEntryPoint>();

        using (_inputManager.LockInputSystem())
        {
            _gameRun.StartRun();

            await UniTask.WhenAll(gameplay.UIFacade.SelectPanel(EGamePanels.Game),
                                  gameplay.PlayerFacade.ShowPlayer());

            _cinemachineService.SetFollow(ECinemachineState.Game, gameplay.PlayerFacade.Transform);
            gameplay.PlayerFacade.ToggleControl(true);
        }

        StatesMachine.Enter<GamePlayingState>();
    }

    public UniTask Exit() => UniTask.CompletedTask;
}
}
