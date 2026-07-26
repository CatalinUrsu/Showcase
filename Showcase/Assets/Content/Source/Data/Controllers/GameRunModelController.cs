using R3;
using IdleNumbers;

namespace Source.Data
{
public class GameRunModelController : IGameRunModelController
{
    public IGameRunModel IModel => _model;

    readonly GameRunModel _model;
    readonly IProgressModelController _progressModelController;
    readonly float _shipCoinBonus;
    readonly CompositeDisposable _disposables = new();
    
    float _lvlCoinBonus;

    public GameRunModelController(IProgressModelController progressModelController, IShipModel shipModel)
    {
        _model = new GameRunModel();
        _progressModelController = progressModelController;
        _shipCoinBonus = shipModel.EnemyCoinBonusRef.CurrentValue / 100 + 1;

        _progressModelController.IModel.LvlRef.Subscribe(SetLvlCoinBonus).AddTo(_disposables);
        _progressModelController.IModel.LvlRef.Skip(1).Subscribe(OnReachNewLvl_handler).AddTo(_disposables);
        _model.ProgressRef.Skip(1).Subscribe(OnProgressChange_handler).AddTo(_disposables);
    }

    public void AddKillReward(float rewardPoints, IdleNumber coins)
    {
        if (!_model.PlayerIsAlive) return;

        var rewardCoins = (_lvlCoinBonus * coins + coins) * _shipCoinBonus;

        _progressModelController.AddCoins(rewardCoins);
        _model.CollectedCoins.Value += rewardCoins;
        _model.Progress.Value += rewardPoints;
    }

    public void StartRun()
    {
        _model.PlayerIsAlive = true;
        _model.Progress.Value = 0;
        _model.CollectedCoins.Value = 0;
    }
    
    public void KillPlayer(bool alive) => _model.PlayerIsAlive = false;

    public void Dispose() => _disposables.Dispose();

    void SetLvlCoinBonus(int lvl) => _lvlCoinBonus = lvl * ConstGameplay.COINS_LVL_BONUS_MULTIPLIER;

    void OnReachNewLvl_handler(int lvl)
    {
        _model.Progress.Value = 0;
        SessionService.Current.Save(ESaveFileType.Progress);
    }
    
    void OnProgressChange_handler(float progress)
    {
        if (progress >= ConstGameplay.PROGRESS_TARGET)
            _progressModelController.AddLevel();
    }
}
}