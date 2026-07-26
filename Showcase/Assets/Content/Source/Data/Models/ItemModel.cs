using R3;
using System;
using IdleNumbers;

namespace Source.Data
{
[Serializable]
public class ItemModel : IItemModel
{

    public ReadOnlyReactiveProperty<bool> IsBoughtRef => IsBought;
    public ReadOnlyReactiveProperty<bool> IsSelectedRef => IsSelected;
    public ReadOnlyReactiveProperty<IdleNumber> BuyPriceRef => BuyPrice;
    public ReadOnlyReactiveProperty<IdleNumber> UpgradePriceRef => UpgradePrice;

    public ReactiveProperty<bool> IsBought { get; private set; }
    public ReactiveProperty<bool> IsSelected { get; private set; }
    public ReactiveProperty<IdleNumber> BuyPrice { get; private set; }
    public ReactiveProperty<IdleNumber> UpgradePrice { get; private set; }

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