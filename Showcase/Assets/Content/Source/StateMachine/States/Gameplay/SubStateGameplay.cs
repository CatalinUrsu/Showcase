using Source.MVP;
using Source.Player;
using Source.Gameplay;
using Source.UI.Gameplay;
using Helpers.StateMachine;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public abstract class SubStateGameplay : IStateEnterPayload<bool>
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    protected readonly GameProgressPresenter _progressPresenter;
    protected readonly PlayerFacadeGameplay _playerFacadeGameplay;
    protected readonly EnemiesSpawner _enemiesSpawner;
    protected readonly GameUIController _uiController;

#endregion

#region Public methods

    protected SubStateGameplay(GameUIController uiController, EnemiesSpawner enemiesSpawner, PlayerFacadeGameplay playerFacadeGameplay, GameProgressPresenter progressPresenter)
    {
        _playerFacadeGameplay = playerFacadeGameplay;
        _progressPresenter = progressPresenter;
        _enemiesSpawner = enemiesSpawner;
        _uiController = uiController;
    }

    public abstract UniTask Enter(bool payload);

    public abstract UniTask Exit();

#endregion
}
}