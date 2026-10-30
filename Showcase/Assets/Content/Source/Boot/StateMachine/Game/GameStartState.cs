using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
/// <summary>
/// Starts new run: resets run model and spawns player. Used for first start and for restart.
/// </summary>
public class GameStartState : IStateEnter
{
    public StatesMachine StatesMachine { get; set; }

    readonly ICinemachineService _cinemachineService;
    readonly IGameRunModelController _gameRunModelController;
    readonly EntryPointRegistry _entryPointRegistry;

    public GameStartState(ICinemachineService cinemachineService,
                          IGameRunModelController gameRunModelController,
                          EntryPointRegistry entryPointRegistry)
    {
        _cinemachineService = cinemachineService;
        _gameRunModelController = gameRunModelController;
        _entryPointRegistry = entryPointRegistry;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameplay = _entryPointRegistry.Get<IGameplayEntryPoint>();

        using (InputManager.Instance.LockInputSystem())
        {
            _gameRunModelController.StartRun();

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
