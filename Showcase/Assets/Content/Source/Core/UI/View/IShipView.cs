using IdleNumbers;

namespace Source
{
public interface IShipView : IItemView
{
    void SetShipStats(IdleNumber upgradePrice, float bonus);
}
}