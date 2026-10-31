using R3;
using System;
using IdleNumbers;

namespace Source.Data
{
/// <summary>
/// Run rules that touch several models: coins reward with level and ship bonuses, level up by run progress.
/// </summary>
public class GameRunService : IDisposable
{
#region Fields

    float _lvlCoinBonus;
    float _shipCoinBonus;

    readonly GameRunModel _gameRun;
    readonly ProgressModel _progress;
    readonly ItemsModel _items;
    readonly GameBalanceSO _balance;
    readonly CompositeDisposable _disposables = new();

#endregion

#region Public methods

    public GameRunService(GameRunModel gameRun, ProgressModel progress, ItemsModel items, GameBalanceSO balance)
    {
        _gameRun = gameRun;
        _progress = progress;
        _items = items;
        _balance = balance;

        SetLvlCoinBonus(_progress.Lvl.CurrentValue);

        _gameRun.Progress.Skip(1).Subscribe(OnProgressChange_handler).AddTo(_disposables);
        _progress.UsedShipIdx.Subscribe(SetShipCoinBonus).AddTo(_disposables);
        _progress.Lvl.Skip(1).Subscribe(OnReachNewLvl_handler).AddTo(_disposables);
    }

    public void AddCoinsReward(float rewardPoints, IdleNumber coins)
    {
        if (_gameRun.PlayerIsKilled.CurrentValue) return;

        var rewardCoins = (_lvlCoinBonus * coins + coins) * _shipCoinBonus;

        _progress.AddCoins(rewardCoins);
        _gameRun.AddReward(rewardCoins, rewardPoints);
    }

    public void Dispose() => _disposables.Dispose();

#endregion

#region Private methods

    void SetShipCoinBonus(int usedShipIdx) => _shipCoinBonus = _items.GetShip(usedShipIdx).EnemyCoinBonus.CurrentValue / 100 + 1;

    void OnReachNewLvl_handler(int lvl)
    {
        SetLvlCoinBonus(lvl);
        _gameRun.ResetProgress();
    }

    void OnProgressChange_handler(float progress)
    {
        if (progress >= _balance.ProgressTarget)
            _progress.AddLevel();
    }

    void SetLvlCoinBonus(int lvl) => _lvlCoinBonus = lvl * _balance.CoinsLvlBonusMultiplier;

#endregion
}
}
