using IdleNumbers;

namespace Source.UI
{
public interface IWeaponView : IItemView
{
    void SetWeaponsStats(IdleNumber upgradePrice, IdleNumber firePower, float fireRate);
}
}