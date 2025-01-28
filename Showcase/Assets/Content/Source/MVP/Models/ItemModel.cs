using R3;
using System;
using IdleNumbers;
using Source.Datas;

namespace Source.MVP
{
[Serializable]
public class ItemModel
{
#region Fields

    ItemInitData _initData;
    public ReactiveProperty<bool> IsBought { get; private set; }
    public ReactiveProperty<bool> IsSelected { get; private set; }
    public ReactiveProperty<IdleNumber> BuyPrice { get; private set; }
    public ReactiveProperty<IdleNumber> UpgradePrice { get; private set; }

#endregion

#region Public methods

    /// <summary>
    /// Constructor for JSON Deserializer for SaveSytem
    /// </summary>
    protected ItemModel()
    {
        IsBought = new ReactiveProperty<bool>(false);
        IsSelected = new ReactiveProperty<bool>(false);
        BuyPrice = new ReactiveProperty<IdleNumber>(new IdleNumber());
        UpgradePrice = new ReactiveProperty<IdleNumber>(new IdleNumber());
    }

    /// <summary>
    /// Constructor for case when is new item and need to set <b>InitData</b> and all <b>properties</b>
    /// </summary>
    public ItemModel(ItemInitData initData)
    {
        SetInitData(initData);
        IsBought = new ReactiveProperty<bool>(initData.IsBought);
        IsSelected = new ReactiveProperty<bool>(initData.IsSelected);
        BuyPrice = new ReactiveProperty<IdleNumber>(initData.BuyPrice);
        UpgradePrice = new ReactiveProperty<IdleNumber>(initData.UpgradePrice);
    }

    public virtual void ResetModel()
    {
        IsBought.Value = _initData.IsBought;
        IsSelected.Value = _initData.IsSelected;
        BuyPrice.Value = _initData.BuyPrice;
        UpgradePrice.Value = _initData.UpgradePrice;
    }

#endregion

    protected void SetInitData(ItemInitData initData) => _initData = initData;
}
}