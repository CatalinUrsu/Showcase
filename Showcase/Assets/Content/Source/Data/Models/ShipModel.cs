using R3;

namespace Source.Data
{
public class ShipModel : ItemModel
{
#region Fields

    public ReadOnlyReactiveProperty<float> EnemyCoinBonus => _enemyCoinBonus;

    readonly ReactiveProperty<float> _enemyCoinBonus;

#endregion

#region Public methods

    public ShipModel(ShipSaveData data) : base(data.IsBought, data.IsSelected, data.BuyPrice, data.UpgradePrice) =>
        _enemyCoinBonus = new ReactiveProperty<float>(data.EnemyCoinBonus);

    public ShipModel(ShipInitData initData) : base(initData.IsBought, initData.IsSelected, initData.BuyPrice, initData.UpgradePrice) =>
        _enemyCoinBonus = new ReactiveProperty<float>(initData.EnemyCoinBonus);

    public void Upgrade(GameBalanceSO balance)
    {
        _upgradePrice.Value *= balance.ShipPriceMultiplier;
        _enemyCoinBonus.Value += balance.ShipBonusIncrease;

        _upgradePrice.ForceNotify();
    }

    public ShipSaveData ToSaveData()
    {
        var data = new ShipSaveData { EnemyCoinBonus = _enemyCoinBonus.Value };
        FillSaveData(data);
        return data;
    }

#endregion
}
}
