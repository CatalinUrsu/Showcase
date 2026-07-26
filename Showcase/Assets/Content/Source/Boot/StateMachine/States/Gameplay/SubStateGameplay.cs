using Cysharp.Threading.Tasks;
using Helpers.Services;
using Source.Data;
using Source.Gameplay;
using Source.Player;
using Source.UI.Gameplay;

namespace Source.Boot
{
public abstract class SubStateGameplay : IStateEnterPayload<bool>
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    protected readonly GameRunPresenter _progressPresenter;
    protected readonly PlayerFacadeGameplay _playerFacadeGameplay;
    protected readonly EnemiesSpawner _enemiesSpawner;
    protected readonly GameUIController _uiController;

#endregion

#region Public methods

    protected SubStateGameplay(GameUIController uiController, EnemiesSpawner enemiesSpawner, PlayerFacadeGameplay playerFacadeGameplay, GameRunPresenter progressPresenter)
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