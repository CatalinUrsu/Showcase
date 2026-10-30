using Helpers;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GamePlayingState : IStateEnter
{
    public StatesMachine StatesMachine { get; set; }

    readonly IGameRunModelController _gameRunModelController;
    readonly EntryPointRegistry _entryPointRegistry;

    public GamePlayingState(IGameRunModelController gameRunModelController, EntryPointRegistry entryPointRegistry)
    {
        _gameRunModelController = gameRunModelController;
        _entryPointRegistry = entryPointRegistry;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        var gameplay = _entryPointRegistry.Get<IGameplayEntryPoint>();

        // After start the panel is already Game (no-op), after pause it switches back
        using (InputManager.Instance.LockInputSystem())
            await gameplay.UIFacade.SelectPanel(EGamePanels.Game);

        _gameRunModelController.OnClickPause += OpenPause;
        _gameRunModelController.OnPlayerLoose += OpenLoose;
    }

    public UniTask Exit()
    {
        _gameRunModelController.OnClickPause -= OpenPause;
        _gameRunModelController.OnPlayerLoose -= OpenLoose;
        return UniTask.CompletedTask;
    }

    void OpenPause() => StatesMachine.Enter<GamePauseState>();

    void OpenLoose() => StatesMachine.Enter<GameLooseState>();
}
}
