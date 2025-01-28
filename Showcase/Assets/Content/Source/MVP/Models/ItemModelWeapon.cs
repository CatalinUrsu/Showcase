using R3;
using System;
using IdleNumbers;
using Source.Datas;

namespace Source.MVP
{
[Serializable]
public class ItemModelWeapon : ItemModel
{
#region Fields

    ItemInitDataWeapon _initData;
    public ReactiveProperty<IdleNumber> FirePower { get; private set; }
    public ReactiveProperty<float> FireRate { get; private set; }

#endregion

#region Public methods

    /// <summary>
    /// Constructor for JSON Deserializer for SaveSytem
    /// </summary>
    public ItemModelWeapon()
    {
        FirePower = new ReactiveProperty<IdleNumber>(new IdleNumber());
        FireRate = new ReactiveProperty<float>();
    }

    /// <summary>
    /// Constructor for case when is new item and need to set <b>InitData</b> and all <b>properties</b>
    /// </summary>
    public ItemModelWeapon(ItemInitDataWeapon initData) : base(initData)
    {
        SetInitData(initData);
        FirePower = new ReactiveProperty<IdleNumber>(initData.FirePower);
        FireRate = new ReactiveProperty<float>(initData.FireRate);
    }

    public void SetInitData(ItemInitDataWeapon initData)
    {
        _initData = initData;
        base.SetInitData(initData);
    }

    public override void ResetModel()
    {
        FirePower.Value = _initData.FirePower;
        FireRate.Value = _initData.FireRate;
        base.ResetModel();
    }

#endregion
}
}