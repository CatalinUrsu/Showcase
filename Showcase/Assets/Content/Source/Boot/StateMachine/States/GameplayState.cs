using Helpers;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
public class GameplayState : IStateEnter
{
    public StatesMachine StatesMachine { get; set; }

    IGameplayContext _gameplayContext;

#region Public methods

    public GameplayState(IGameplayContext gameplayContext) => _gameplayContext = gameplayContext;

    public async UniTask Enter()
    {
        AddGameRunControllerListeners();
        _gameplayContext.EnemiesController.ToggleSpawning(true);

        await UniTask.WhenAll(_gameplayContext.UIFacade.SelectPanel(EGamePanels.Game),
                              _gameplayContext.PlayerFacade.ShowPlayer());

        _gameplayContext.PlayerFacade.ToggleControl(true);
    }

    public async UniTask Exit()
    {
        RemoveGameRunControllerListeners();
        _gameplayContext.EnemiesController.ToggleSpawning(false);
        _gameplayContext.PlayerFacade.ToggleControl(false);
    }

#endregion

    void AddGameRunControllerListeners()
    {
        _gameplayContext.GameRunModelController.OnClickPause += GoToPauseState;
        _gameplayContext.GameRunModelController.OnPlayerLoose += GoToLooseState;
    }

    void RemoveGameRunControllerListeners()
    {
        _gameplayContext.GameRunModelController.OnClickPause -= GoToPauseState;
        _gameplayContext.GameRunModelController.OnPlayerLoose -= GoToLooseState;
    }
    
    void GoToPauseState()
    {
        GoToPause_Async().Forget();
        return;

        async UniTaskVoid GoToPause_Async()
        {
            using (InputManager.Instance.LockInputSystem())
                await StatesMachine.Enter<GameMenuPauseState>();
        }
    }

    void GoToLooseState()
    {
        GoToLoose_Async().Forget();
        return;

        async UniTaskVoid GoToLoose_Async()
        {
            using (InputManager.Instance.LockInputSystem())
                await StatesMachine.Enter<GameMenuLooseState>();
        }
    }
}
}