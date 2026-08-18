using R3;
using System;
using IdleNumbers;
using System.Text.Json.Serialization;

namespace Source.Data
{
[Serializable]
public class ItemModel : IItemModel
{

    [JsonIgnore] public ReadOnlyReactiveProperty<bool> IsBoughtRef => IsBought;
    [JsonIgnore] public ReadOnlyReactiveProperty<bool> IsSelectedRef => IsSelected;
    [JsonIgnore] public ReadOnlyReactiveProperty<IdleNumber> BuyPriceRef => BuyPrice;
    [JsonIgnore] public ReadOnlyReactiveProperty<IdleNumber> UpgradePriceRef => UpgradePrice;

    [JsonInclude] public ReactiveProperty<bool> IsBought { get; private set; }
    [JsonInclude] public ReactiveProperty<bool> IsSelected { get; private set; }
    [JsonInclude] public ReactiveProperty<IdleNumber> BuyPrice { get; private set; }
    [JsonInclude] public ReactiveProperty<IdleNumber> UpgradePrice { get; private set; }

    protected ItemModel()
    {
        IsBought = new ReactiveProperty<bool>(false);
        IsSelected = new ReactiveProperty<bool>(false);
        BuyPrice = new ReactiveProperty<IdleNumber>(new IdleNumber());
        UpgradePrice = new ReactiveProperty<IdleNumber>(new IdleNumber());
    }

    public ItemModel(ItemInitData initData)
    {
        IsBought = new ReactiveProperty<bool>(initData.IsBought);
        IsSelected = new ReactiveProperty<bool>(initData.IsSelected);
        BuyPrice = new ReactiveProperty<IdleNumber>(initData.BuyPrice);
        UpgradePrice = new ReactiveProperty<IdleNumber>(initData.UpgradePrice);
    }
}
}