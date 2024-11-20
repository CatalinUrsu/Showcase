using IdleNumbers;

namespace Source.MVP
{
public interface IViewItemShip
{
    void OnUpgrade_handler(IdleNumber upgradePrice, float bonus);
}
}