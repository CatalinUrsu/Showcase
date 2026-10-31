using R3;
using IdleNumbers;

namespace Source.Data
{
/// <summary>
/// Common state of shop item: purchase, selection and prices.
/// </summary>
public abstract class ItemModel
{
#region Fields

    public ReadOnlyReactiveProperty<bool> IsBought => _isBought;
    public ReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
    public ReadOnlyReactiveProperty<IdleNumber> BuyPrice => _buyPrice;
    public ReadOnlyReactiveProperty<IdleNumber> UpgradePrice => _upgradePrice;

    public IdleNumber CurrentPrice => _isBought.Value ? _upgradePrice.Value : _buyPrice.Value;

    protected readonly ReactiveProperty<IdleNumber> _upgradePrice;

    readonly ReactiveProperty<bool> _isBought;
    readonly ReactiveProperty<bool> _isSelected;
    readonly ReactiveProperty<IdleNumber> _buyPrice;

#endregion

#region Public methods

    protected ItemModel(bool isBought, bool isSelected, IdleNumber buyPrice, IdleNumber upgradePrice)
    {
        _isBought = new ReactiveProperty<bool>(isBought);
        _isSelected = new ReactiveProperty<bool>(isSelected);
        _buyPrice = new ReactiveProperty<IdleNumber>(buyPrice);
        _upgradePrice = new ReactiveProperty<IdleNumber>(upgradePrice);
    }

    public void Buy() => _isBought.Value = true;

    public void Select() => _isSelected.Value = true;

    public void Deselect() => _isSelected.Value = false;

#endregion

#region Private methods

    protected void ResetItem(ItemInitData initData)
    {
        _isBought.Value = initData.IsBought;
        _isSelected.Value = initData.IsSelected;
        _buyPrice.Value = initData.BuyPrice;
        _upgradePrice.Value = initData.UpgradePrice;
    }

    protected void FillSaveData(ItemSaveData data)
    {
        data.IsBought = _isBought.Value;
        data.IsSelected = _isSelected.Value;
        data.BuyPrice = _buyPrice.Value;
        data.UpgradePrice = _upgradePrice.Value;
    }

#endregion
}
}
