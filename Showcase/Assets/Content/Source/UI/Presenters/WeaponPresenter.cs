using R3;
using System;
using Zenject;
using Source.Data;

namespace Source.UI
{
public class WeaponPresenter : IPresenterItemBase
{
#region External Types

    public class Factory : PlaceholderFactory<IWeaponView, Guid, WeaponPresenter> { }

#endregion

#region Fields

    readonly Guid _key;
    readonly IShopService _shopService;
    readonly CompositeDisposable _disposables = new();

#endregion

#region Public methods

    public WeaponPresenter(IWeaponView view, Guid key, IShopService shopService, ItemsModel items, ProgressModel progress)
    {
        _key = key;
        _shopService = shopService;

        var weapon = items.GetWeapon(key);

        weapon.IsBought.Subscribe(isBought => view.UpdateBoughtState(isBought, weapon.CurrentPrice)).AddTo(_disposables);
        weapon.IsSelected.Subscribe(isSelected => view.UpdateSelectState(weapon.IsBought.CurrentValue, isSelected)).AddTo(_disposables);
        weapon.UpgradePrice.Subscribe(_ =>
        {
            view.SetWeaponsStats(weapon.CurrentPrice, weapon.FirePower.CurrentValue, weapon.FireRate.CurrentValue);
            view.UpdateSolvency(progress.Coins.CurrentValue.IsEnough(weapon.CurrentPrice));
        }).AddTo(_disposables);
        progress.Coins.Subscribe(coins => view.UpdateSolvency(coins.IsEnough(weapon.CurrentPrice))).AddTo(_disposables);
    }

    public void BuyOrUpgradeItem() => _shopService.TryBuyOrUpgradeWeapon(_key);

    public void SelectItem() => _shopService.TrySelectWeapon(_key);

    public void Dispose() => _disposables.Dispose();

#endregion
}
}
