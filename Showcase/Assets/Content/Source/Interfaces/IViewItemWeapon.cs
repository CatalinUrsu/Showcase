using IdleNumbers;

namespace Source.MVP
{
public interface IViewItemWeapon
{
    void OnUpgrade_handler(IdleNumber upgradePrice, IdleNumber firePower, float fireRate);
}
}