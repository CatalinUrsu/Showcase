using R3;
using System;

namespace Source.Data
{
[Serializable]
public class ShipModel : ItemModel, IShipModel
{
    public ReadOnlyReactiveProperty<float> EnemyCoinBonusRef => EnemyCoinBonus;
    public ReactiveProperty<float> EnemyCoinBonus { get; private set; }

    public ShipModel() => EnemyCoinBonus = new ReactiveProperty<float>();
    public ShipModel(ShipInitData initData) : base(initData) => EnemyCoinBonus = new ReactiveProperty<float>(initData.EnemyCoinBonus);
}
}