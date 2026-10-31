using R3;
using System;

namespace Source.Data
{
/// <summary>
/// Shop rules: spend currency, buy or upgrade, select bought item and deselect previous one.
/// Ships are paid with diamonds, weapons with coins.
/// </summary>
public class ShopService : IShopService, IDisposable
{
#region Fields

    public Observable<EShopAction> Actions => _actions;

    readonly ItemsModel _items;
    readonly ProgressModel _progress;
    readonly GameBalanceSO _balance;
    readonly Subject<EShopAction> _actions = new();

#endregion

#region Public methods

    public ShopService(ItemsModel items, ProgressModel progress, GameBalanceSO balance)
    {
        _items = items;
        _progress = progress;
        _balance = balance;
    }

    public bool TryBuyOrUpgradeShip(Guid id)
    {
        var ship = _items.GetShip(id);
        if (!_progress.TrySpendDiamonds(ship.CurrentPrice)) return false;

        if (ship.IsBought.CurrentValue)
        {
            ship.Upgrade(_balance);
            _actions.OnNext(EShopAction.Upgraded);
        }
        else
        {
            ship.Buy();
            _actions.OnNext(EShopAction.Bought);
            TrySelectShip(id);
        }

        return true;
    }

    public bool TrySelectShip(Guid id)
    {
        var ship = _items.GetShip(id);
        if (!ship.IsBought.CurrentValue || ship.IsSelected.CurrentValue) return false;

        _items.GetShip(_progress.UsedShipIdx.CurrentValue).Deselect();
        ship.Select();
        _progress.SetUsedShipIdx(_items.GetShipIdx(id));

        _actions.OnNext(EShopAction.Selected);
        return true;
    }

    public bool TryBuyOrUpgradeWeapon(Guid id)
    {
        var weapon = _items.GetWeapon(id);
        if (!_progress.TrySpendCoins(weapon.CurrentPrice)) return false;

        if (weapon.IsBought.CurrentValue)
        {
            weapon.Upgrade(_balance);
            _actions.OnNext(EShopAction.Upgraded);
        }
        else
        {
            weapon.Buy();
            _actions.OnNext(EShopAction.Bought);
            TrySelectWeapon(id);
        }

        return true;
    }

    public bool TrySelectWeapon(Guid id)
    {
        var weapon = _items.GetWeapon(id);
        if (!weapon.IsBought.CurrentValue || weapon.IsSelected.CurrentValue) return false;

        _items.GetWeapon(_progress.UsedWeaponIdx.CurrentValue).Deselect();
        weapon.Select();
        _progress.SetUsedWeaponIdx(_items.GetWeaponIdx(id));

        _actions.OnNext(EShopAction.Selected);
        return true;
    }

    public void Dispose() => _actions.Dispose();

#endregion
}
}
