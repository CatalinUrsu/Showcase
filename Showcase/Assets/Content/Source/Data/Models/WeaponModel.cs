using R3;
using System;
using IdleNumbers;
using System.Text.Json.Serialization;

namespace Source.Data
{
[Serializable]
public class WeaponModel : ItemModel, IWeaponModel
{
    [JsonIgnore] public ReadOnlyReactiveProperty<IdleNumber> FirePowerRef => FirePower;
    [JsonIgnore] public ReadOnlyReactiveProperty<float> FireRateRef => FireRate;

    [JsonInclude] public ReactiveProperty<IdleNumber> FirePower { get; private set; }
    [JsonInclude] public ReactiveProperty<float> FireRate { get; private set; }

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