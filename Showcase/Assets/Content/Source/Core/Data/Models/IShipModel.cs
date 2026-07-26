using R3;

namespace Source
{
    /// <summary>
    /// Ship-specific read-only properties.
    /// </summary>
    /// <remarks>
    /// <b>Obtain instances only from <see cref="IItemsModelController"/>.</b>
    /// Do not construct, resolve or cache implementations directly. <br/>
    /// </remarks>
    public interface IShipModel : IItemModel
    {
    public ReadOnlyReactiveProperty<float> EnemyCoinBonusRef { get; }
}
}