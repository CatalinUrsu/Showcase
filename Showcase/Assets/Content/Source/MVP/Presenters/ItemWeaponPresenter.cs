using R3;
using IdleNumbers;
using UnityEngine;
using System.Linq;
using Source.Audio;
using Helpers.Audio;
using Source.Session;

namespace Source.MVP
{
public class ItemWeaponPresenter : IPresenterItemBase
{
#region Fields

    IViewItem _itemView;
    IViewItemWeapon _weaponView;
    ItemModelWeapon _weaponModel;
    int _itemIdx;

#endregion

#region Public methods

    public ItemWeaponPresenter(GameObject weaponGO, IViewItem itemView, IViewItemWeapon weaponView, int itemIdx)
    {
        _itemView = itemView;
        _weaponView = weaponView;
        _weaponModel = SessionManager.Current.Items.Weapons.ElementAt(itemIdx).Value;
        _itemIdx = itemIdx;

        SubscribeProperties(weaponGO);
    }

    public void BuyOrUpgradeItem()
    {
        if (!SessionManager.Current.Progress.Coins.Value.IsEnough(GetPrice())) return;
        
        FmodEvents.Instance.BtnBuy.PlayOneShot();

        if (!_weaponModel.IsBought.Value)
            BuyItem();
        else
            UpgradeItem();
    }

    public void SelectItem()
    {
        if (!_weaponModel.IsBought.Value || _weaponModel.IsSelected.Value) return;
        
        FmodEvents.Instance.BtnSelect.PlayOneShot();
        
        _weaponModel.IsSelected.Value = true;
        SessionManager.Current.Progress.UsedWeaponIdx.Value = _itemIdx;
        SessionManager.Current.Save(ESaveFileType.Items);
        SessionManager.Current.Save(ESaveFileType.Progress);
    }

#endregion

#region Private methods

    void SubscribeProperties(GameObject itemGO)
    {
        _weaponModel.IsBought.Subscribe(isBought => _itemView.OnChangeBoughtState_handler(isBought, GetPrice())).AddTo(itemGO);
        _weaponModel.IsSelected.Subscribe(isSelect => _itemView.OnChangeSelectState_handler(_weaponModel.IsBought.Value, isSelect)).AddTo(itemGO);
        _weaponModel.UpgradePrice.Subscribe(_ => _weaponView.OnUpgrade_handler(GetPrice(), _weaponModel.FirePower.Value, _weaponModel.FireRate.Value)).AddTo(itemGO);
        SessionManager.Current.Progress.Coins.Subscribe(_ => _itemView.OnUpdateSolvency_handler(SessionManager.Current.Progress.Coins.Value.IsEnough(GetPrice()))).AddTo(itemGO);
        SessionManager.Current.Progress.UsedWeaponIdx.Subscribe(DeselectOnSelectOtherItem).AddTo(itemGO);
    }

    void BuyItem()
    {
        SessionManager.Current.Progress.Coins.Value -= _weaponModel.BuyPrice.Value;
        _weaponModel.IsBought.Value = true;
        SelectItem();
    }

    void UpgradeItem()
    {
        var coinsToSpend = _weaponModel.UpgradePrice.Value;
        _weaponModel.FirePower.Value += ConstUpgradeItems.WEAPON_POWER_UPGRADE;
        _weaponModel.FireRate.Value = Mathf.Clamp(_weaponModel.FireRate.Value + ConstUpgradeItems.WEAPON_FIRE_RATE_UPGRADE, ConstUpgradeItems.WEAPON_FIRE_RATE_MIN, 5);
        _weaponModel.UpgradePrice.Value *= ConstUpgradeItems.WEAPON_PRICE_MULTIPLIER;

        SessionManager.Current.Progress.Coins.Value -= coinsToSpend;
        SessionManager.Current.Save(ESaveFileType.Items);
        SessionManager.Current.Save(ESaveFileType.Progress);
    }

    void DeselectOnSelectOtherItem(int selectedItemIdx)
    {
        if (selectedItemIdx == _itemIdx) return;

        _weaponModel.IsSelected.Value = false;
    }

    IdleNumber GetPrice() => _weaponModel.IsBought.Value ? _weaponModel.UpgradePrice.Value : _weaponModel.BuyPrice.Value;

#endregion
}
}