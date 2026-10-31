using R3;
using IdleNumbers;
using UnityEngine;

namespace Source.Data
{
public class WeaponModel : ItemModel
{
#region Fields

    public ReadOnlyReactiveProperty<IdleNumber> FirePower => _firePower;
    public ReadOnlyReactiveProperty<float> FireRate => _fireRate;

    readonly ReactiveProperty<IdleNumber> _firePower;
    readonly ReactiveProperty<float> _fireRate;

#endregion

#region Public methods

    public WeaponModel(WeaponSaveData data) : base(data.IsBought, data.IsSelected, data.BuyPrice, data.UpgradePrice)
    {
        _firePower = new ReactiveProperty<IdleNumber>(data.FirePower);
        _fireRate = new ReactiveProperty<float>(data.FireRate);
    }

    public WeaponModel(WeaponInitData initData) : base(initData.IsBought, initData.IsSelected, initData.BuyPrice, initData.UpgradePrice)
    {
        _firePower = new ReactiveProperty<IdleNumber>(initData.FirePower);
        _fireRate = new ReactiveProperty<float>(initData.FireRate);
    }

    public void Upgrade(GameBalanceSO balance)
    {
        _upgradePrice.Value *= balance.WeaponPriceMultiplier;
        _firePower.Value += balance.WeaponPowerUpgrade;
        _fireRate.Value = Mathf.Clamp(_fireRate.Value + balance.WeaponFireRateUpgrade, balance.WeaponFireRateMin, 1);

        _upgradePrice.ForceNotify();
    }

    public void Reset(WeaponInitData initData)
    {
        ResetItem(initData);
        _firePower.Value = initData.FirePower;
        _fireRate.Value = initData.FireRate;

        _upgradePrice.ForceNotify();
    }

    public WeaponSaveData ToSaveData()
    {
        var data = new WeaponSaveData { FirePower = _firePower.Value, FireRate = _fireRate.Value };
        FillSaveData(data);
        return data;
    }

#endregion
}
}
