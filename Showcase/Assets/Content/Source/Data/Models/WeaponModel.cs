using R3;
using System;
using IdleNumbers;

namespace Source.Data
{
[Serializable]
public class WeaponModel : ItemModel, IWeaponModel
{
    public ReadOnlyReactiveProperty<IdleNumber> FirePowerRef => FirePower;
    public ReadOnlyReactiveProperty<float> FireRateRef => FireRate;

    public ReactiveProperty<IdleNumber> FirePower { get; private set; }
    public ReactiveProperty<float> FireRate { get; private set; }

    public WeaponModel()
    {
        FirePower = new ReactiveProperty<IdleNumber>(new IdleNumber());
        FireRate = new ReactiveProperty<float>();
    }

    public WeaponModel(WeaponInitData initData) : base(initData)
    {
        FirePower = new ReactiveProperty<IdleNumber>(initData.FirePower);
        FireRate = new ReactiveProperty<float>(initData.FireRate);
    }
}
}