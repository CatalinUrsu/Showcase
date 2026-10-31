using R3;
using System;
using UnityEngine;
using Zenject;

namespace Source.Data
{
public class WeaponPresenter : ItemPresenterBase
{
#region External Types

    public class Factory : PlaceholderFactory<IWeaponView, Guid, WeaponPresenter> { }

#endregion
    
#region Fields

    readonly IWeaponView _view;
    readonly WeaponModel _weaponModel;

#endregion

#region Public methods

    public WeaponPresenter(IWeaponView view,
                           Guid key,
                           ISessionService sessionService,
                           ItemsModel items,
                           ProgressModel progress,
                           GameBalanceSO balance,
                           FmodEventsSo fmodEventsSo) :
        base(key, sessionService, items, progress, balance, fmodEventsSo)
    {
        _view = view;
        _weaponModel = _items.GetWeapon(key);
        _idx = _items.GetWeaponIdx(_key);
        _itemModel = _weaponModel;

        // Subscribe to Properties value changing
        _weaponModel.IsBought.Subscribe(isBought => _view.UpdateBoughtState(isBought, GetPrice())).AddTo(_disposables);
        _weaponModel.IsSelected.Subscribe(isSelect => _view.UpdateSelectState(_weaponModel.IsBought.CurrentValue, isSelect)).AddTo(_disposables);
        _weaponModel.UpgradePrice.Subscribe(_ => SetWeaponsStats()).AddTo(_disposables);

        _progress.Coins.Subscribe(_ => _view.UpdateSolvency(HasEnoughCurrency())).AddTo(_disposables);
        _progress.UsedWeaponIdx.Subscribe(DeselectOnSelectOtherItem).AddTo(_disposables);
    }

    public override void SelectItem()
    {
        if (!CanSelectItem()) return;

        _weaponModel.Select();
        _progress.SetUsedWeaponIdx(_idx);

        base.SelectItem();
    }

#endregion

#region Private methods

    protected override void BuyItem()
    {
        if (!_progress.TrySpendCoins(_weaponModel.BuyPrice.CurrentValue)) return;
        _weaponModel.Buy();

        base.BuyItem();
        SelectItem();
    }

    protected override void UpgradeItem()
    {
        if (!_progress.TrySpendCoins(_weaponModel.UpgradePrice.CurrentValue)) return;
        _weaponModel.Upgrade(_balance);

        base.UpgradeItem();
    }

    protected override void DeselectOnSelectOtherItem(int selectedItemIdx)
    {
        if (selectedItemIdx == _idx || !_itemModel.IsSelected.CurrentValue) return;

        _weaponModel.Deselect();
    }

    void SetWeaponsStats()
    {
        _view.SetWeaponsStats(GetPrice(), _weaponModel.FirePower.CurrentValue, _weaponModel.FireRate.CurrentValue);
        _view.UpdateSolvency(HasEnoughCurrency());
    }

#endregion
}
}