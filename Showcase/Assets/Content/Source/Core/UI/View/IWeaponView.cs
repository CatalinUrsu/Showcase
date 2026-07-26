using IdleNumbers;

namespace Source
{
public interface IWeaponView : IItemView
{
    void SetWeaponsStats(IdleNumber upgradePrice, IdleNumber firePower, float fireRate);
}
}