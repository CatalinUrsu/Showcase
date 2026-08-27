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
    readonly IWeaponModel _weaponModel;

#endregion

#region Public methods

    public WeaponPresenter(IWeaponView view,
                           Guid key,
                           ISessionService sessionService,
                           IItemsModelController itemsModelController,
                           IProgressModelController progressModelController,
                           FmodEventsSo fmodEventsSo) :
        base(key, sessionService, itemsModelController, progressModelController, fmodEventsSo)
    {
        _view = view;
        _weaponModel = _itemsController.GetWeaponModel(key);
        _idx = _itemsController.GetWeaponIdx(_key);
        _itemModel = _weaponModel;

        // Subscribe to Properties value changing
        _weaponModel.IsBoughtRef.Subscribe(isBought => _view.UpdateBoughtState(isBought, GetPrice())).AddTo(_disposables);
        _weaponModel.IsSelectedRef.Subscribe(isSelect => _view.UpdateSelectState(_weaponModel.IsBoughtRef.CurrentValue, isSelect)).AddTo(_disposables);
        _weaponModel.UpgradePriceRef.Subscribe(_ => SetWeaponsStats()).AddTo(_disposables);

        _progressModel.CoinsRef.Subscribe(_ => _view.UpdateSolvency(HasEnoughCurrency())).AddTo(_disposables);
        _progressModel.UsedWeaponIdxRef.Subscribe(DeselectOnSelectOtherItem).AddTo(_disposables);
    }

    public override void SelectItem()
    {
        if (!CanSelectItem()) return;

        _itemsController.SelectWeapon(_key);
        _progressController.SetUsedWeaponIdx(_idx);

        base.SelectItem();
    }

#endregion

#region Private methods

    protected override void BuyItem()
    {
        _progressController.SpendCoins(_weaponModel.BuyPriceRef.CurrentValue);
        _itemsController.BuyWeapon(_key);

        base.BuyItem();
        SelectItem();
    }

    protected override void UpgradeItem()
    {
        _progressController.SpendCoins(_weaponModel.UpgradePriceRef.CurrentValue);
        _itemsController.UpdateWeapon(_key);

        base.UpgradeItem();
    }

    protected override void DeselectOnSelectOtherItem(int selectedItemIdx)
    {
        if (selectedItemIdx == _idx || !_itemModel.IsSelectedRef.CurrentValue) return;

        _itemsController.DeselectWeapon(_key);
    }

    void SetWeaponsStats()
    {
        _view.SetWeaponsStats(GetPrice(), _weaponModel.FirePowerRef.CurrentValue, _weaponModel.FireRateRef.CurrentValue);
        _view.UpdateSolvency(HasEnoughCurrency());
    }

#endregion
}
}