using R3;
using System;
using System.Text.Json.Serialization;

namespace Source.Data
{
[Serializable]
public class ShipModel : ItemModel, IShipModel
{
    [JsonIgnore] public ReadOnlyReactiveProperty<float> EnemyCoinBonusRef => EnemyCoinBonus;
    [JsonInclude] public ReactiveProperty<float> EnemyCoinBonus { get; private set; }

    public ShipModel() => EnemyCoinBonus = new ReactiveProperty<float>();
    public ShipModel(ShipInitData initData) : base(initData) => EnemyCoinBonus = new ReactiveProperty<float>(initData.EnemyCoinBonus);
}
}