using Source.MVP;
using StateMachine;
using Source.Player;
using Source.Gameplay;
using Source.UI.Gameplay;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public abstract class StateGameplay_Base : IStateEnterPayload<bool>
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    protected readonly GameProgressPresenter _progressPresenter;
    protected readonly PlayerFacadeGameplay _playerFacadeGameplay;
    protected readonly EnemiesSpawner _enemiesSpawner;
    protected readonly GameUIController _uiController;

#endregion

#region Public methods

    public StateGameplay_Base(GameUIController uiController, EnemiesSpawner enemiesSpawner, PlayerFacadeGameplay playerFacadeGameplay, GameProgressPresenter progressPresenter)
    {
        _playerFacadeGameplay = playerFacadeGameplay;
        _progressPresenter = progressPresenter;
        _enemiesSpawner = enemiesSpawner;
        _uiController = uiController;
    }

    public abstract UniTaskVoid Enter(bool payload);

    public abstract UniTask Exit();

#endregion
}
}