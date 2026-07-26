using R3;
using IdleNumbers;

namespace Source
{
    /// <summary>
    /// Weapon-specific read-only properties.
    /// </summary>
    /// <remarks>
    /// <b>Obtain instances only from <see cref="IItemsModelController"/>.</b>
    /// Do not construct, resolve or cache implementations directly. <br/>
    /// </remarks>
    public interface IWeaponModel : IItemModel
    {
    public ReadOnlyReactiveProperty<IdleNumber> FirePowerRef { get; }
    public ReadOnlyReactiveProperty<float> FireRateRef { get; }
}
}