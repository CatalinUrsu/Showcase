using System;
using Source.Datas;
using UniRx;

namespace Source.MVP
{
[Serializable]
public class ItemModelShip : ItemModel
{
#region Fields

    ItemInitDataShip _initData;
    public ReactiveProperty<float> EnemyCoinBonus { get; private set; }

#endregion

#region Public methods

    /// <summary>
    /// Constructor for JSON Deserializer for SaveSytem
    /// </summary>
    public ItemModelShip()
    {
        EnemyCoinBonus = new ReactiveProperty<float>();
    }

    /// <summary>
    /// Constructor for case when is new item and need to set <b>InitData</b> and all <b>properties</b>
    /// </summary>
    public ItemModelShip(ItemInitDataShip initData) : base(initData)
    {
        SetInitData(initData);
        EnemyCoinBonus = new ReactiveProperty<float>(initData.EnemyCoinBonus);
    }

    public void SetInitData(ItemInitDataShip initData)
    {
        _initData = initData;
        base.SetInitData(initData);
    }

    public override void ResetModel()
    {
        EnemyCoinBonus.Value = _initData.EnemyCoinBonus;
        base.ResetModel();
    }

#endregion
}
}