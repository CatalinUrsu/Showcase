using R3;
using IdleNumbers;

namespace Source
{
/// <summary>
/// Model expose level, coins, diamonds, ets as read-only reactive properties.
/// </summary>
/// <remarks>
/// <b>Obtain instances only from <see cref="IProgressModelController"/>.</b>
/// Do not construct, resolve or cache implementations directly. <br/>
/// </remarks>
public interface IProgressModel
{
    public ReadOnlyReactiveProperty<int> LvlRef { get; }
    public ReadOnlyReactiveProperty<IdleNumber> CoinsRef { get; }
    public ReadOnlyReactiveProperty<IdleNumber> DiamondsRef { get; }
    public ReadOnlyReactiveProperty<int> UsedShipIdxRef { get; }
    public ReadOnlyReactiveProperty<int> UsedWeaponIdxRef { get; }
}
}