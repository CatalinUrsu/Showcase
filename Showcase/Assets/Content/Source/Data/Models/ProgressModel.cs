using R3;
using System;
using IdleNumbers;

namespace Source.Data
{
public class ProgressModel
{
#region Fields

    public ReadOnlyReactiveProperty<int> Lvl => _lvl;
    public ReadOnlyReactiveProperty<IdleNumber> Coins => _coins;
    public ReadOnlyReactiveProperty<IdleNumber> Diamonds => _diamonds;
    public ReadOnlyReactiveProperty<Guid> UsedShipId => _usedShipId;
    public ReadOnlyReactiveProperty<Guid> UsedWeaponId => _usedWeaponId;

    /// <summary>Emits on any change after creation, used by SaveService to mark save dirty.</summary>
    public Observable<Unit> Changed { get; }

    readonly ReactiveProperty<int> _lvl;
    readonly ReactiveProperty<IdleNumber> _coins;
    readonly ReactiveProperty<IdleNumber> _diamonds;
    readonly ReactiveProperty<Guid> _usedShipId;
    readonly ReactiveProperty<Guid> _usedWeaponId;

#endregion

#region Public methods

    public ProgressModel(ProgressSaveData data)
    {
        _lvl = new ReactiveProperty<int>(data.Lvl);
        _coins = new ReactiveProperty<IdleNumber>(data.Coins);
        _diamonds = new ReactiveProperty<IdleNumber>(data.Diamonds);
        _usedShipId = new ReactiveProperty<Guid>(data.UsedShipId);
        _usedWeaponId = new ReactiveProperty<Guid>(data.UsedWeaponId);

        Changed = Observable.Merge(_lvl.Skip(1).AsUnitObservable(),
                                   _coins.Skip(1).AsUnitObservable(),
                                   _diamonds.Skip(1).AsUnitObservable(),
                                   _usedShipId.Skip(1).AsUnitObservable(),
                                   _usedWeaponId.Skip(1).AsUnitObservable());
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

    public void SetUsedShipId(Guid id) => _usedShipId.Value = id;

    public void SetUsedWeaponId(Guid id) => _usedWeaponId.Value = id;

    public void Ascend(int diamondsBonus)
    {
        _lvl.Value = 1;
        _coins.Value = 0;
        _usedWeaponId.Value = Guid.Empty;
        _diamonds.Value += diamondsBonus;
    }

    public ProgressSaveData ToSaveData()
    {
        return new ProgressSaveData
        {
            Lvl = _lvl.Value,
            Coins = _coins.Value,
            Diamonds = _diamonds.Value,
            UsedShipId = _usedShipId.Value,
            UsedWeaponId = _usedWeaponId.Value
        };
    }

#endregion
}
}
