using R3;
using IdleNumbers;

namespace Source.Data
{
public class ProgressModel
{
#region Fields

    public ReadOnlyReactiveProperty<int> Lvl => _lvl;
    public ReadOnlyReactiveProperty<IdleNumber> Coins => _coins;
    public ReadOnlyReactiveProperty<IdleNumber> Diamonds => _diamonds;
    public ReadOnlyReactiveProperty<int> UsedShipIdx => _usedShipIdx;
    public ReadOnlyReactiveProperty<int> UsedWeaponIdx => _usedWeaponIdx;

    readonly ReactiveProperty<int> _lvl;
    readonly ReactiveProperty<IdleNumber> _coins;
    readonly ReactiveProperty<IdleNumber> _diamonds;
    readonly ReactiveProperty<int> _usedShipIdx;
    readonly ReactiveProperty<int> _usedWeaponIdx;

#endregion

#region Public methods

    public ProgressModel(ProgressSaveData data)
    {
        _lvl = new ReactiveProperty<int>(data.Lvl);
        _coins = new ReactiveProperty<IdleNumber>(data.Coins);
        _diamonds = new ReactiveProperty<IdleNumber>(data.Diamonds);
        _usedShipIdx = new ReactiveProperty<int>(data.UsedShipIdx);
        _usedWeaponIdx = new ReactiveProperty<int>(data.UsedWeaponIdx);
    }

    public void AddLevel() => _lvl.Value++;

    public void AddCoins(IdleNumber value) => _coins.Value += value;

    public void AddDiamonds(IdleNumber value) => _diamonds.Value += value;

    public bool TrySpendCoins(IdleNumber price)
    {
        if (!_coins.Value.IsEnough(price)) return false;

        _coins.Value -= price;
        return true;
    }

    public bool TrySpendDiamonds(IdleNumber price)
    {
        if (!_diamonds.Value.IsEnough(price)) return false;

        _diamonds.Value -= price;
        return true;
    }

    public void SetUsedShipIdx(int idx) => _usedShipIdx.Value = idx;

    public void SetUsedWeaponIdx(int idx) => _usedWeaponIdx.Value = idx;

    public void Ascend(int diamondsBonus)
    {
        _lvl.Value = 1;
        _coins.Value = 0;
        _usedWeaponIdx.Value = 0;
        _diamonds.Value += diamondsBonus;
    }

    public ProgressSaveData ToSaveData()
    {
        return new ProgressSaveData
        {
            Lvl = _lvl.Value,
            Coins = _coins.Value,
            Diamonds = _diamonds.Value,
            UsedShipIdx = _usedShipIdx.Value,
            UsedWeaponIdx = _usedWeaponIdx.Value
        };
    }

#endregion
}
}
