using UniRx;
using System.Linq;
using IdleNumbers;
using Source.Session;

namespace Source.MVP
{
public class GameProgressModel
{
    public ReactiveProperty<float> Progress { get; private set; }
    public ReactiveProperty<IdleNumber> CollectedCoins { get; private set; }
    
    public bool PlayerIsAlive;
    public IdleNumber RewardCoins;

    readonly bool _vibrationOn;
    readonly float _shipCoinBonus;
    SessionProgress _sessionProgress;

    public GameProgressModel()
    {
        var sessionManager = SessionManager.Current;
        _sessionProgress = sessionManager.Progress;
        _shipCoinBonus = sessionManager.Items.Ships.ElementAt(_sessionProgress.UsedShipIdx.Value).Value.EnemyCoinBonus.Value / 100 + 1;

        Progress = new(0);
        CollectedCoins = new(new IdleNumber());
    }

    public void AddKillReward(float rewardPoints, IdleNumber coins)
    {
        if (!PlayerIsAlive) return;

        RewardCoins = (_sessionProgress.Lvl.Value * ConstGameplay.COINS_LVL_BONUS_MULTIPLIER * coins + coins) * _shipCoinBonus;

        _sessionProgress.Coins.Value += RewardCoins;
        CollectedCoins.Value += RewardCoins;
        Progress.Value += rewardPoints;
    }
}
}