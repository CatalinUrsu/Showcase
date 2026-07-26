using R3;
using IdleNumbers;

namespace Source
{
    /// <summary>
    /// Base interface for ItemModel, with common properties (purchase state, selection state, and pricing).
    /// </summary>
    /// <remarks>
    /// <b>Obtain instances only from <see cref="IItemsModelController"/>.</b>
    /// Do not construct, resolve or cache implementations directly. <br/>
    /// </remarks>
    public interface IItemModel
    {
        public ReadOnlyReactiveProperty<bool> IsBoughtRef { get; }
        public ReadOnlyReactiveProperty<bool> IsSelectedRef { get; }
        public ReadOnlyReactiveProperty<IdleNumber> BuyPriceRef { get; }
        public ReadOnlyReactiveProperty<IdleNumber> UpgradePriceRef { get; }
    }
}