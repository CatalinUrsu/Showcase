using IdleNumbers;

namespace Source.UI
{
public interface IShipView : IItemView
{
    void SetShipStats(IdleNumber upgradePrice, float bonus);
}
}