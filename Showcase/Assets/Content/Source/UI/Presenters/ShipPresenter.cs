using R3;
using System;
using Zenject;
using Source.Data;

namespace Source.UI
{
public class ShipPresenter : IPresenterItemBase
{
#region External Types

    public class Factory : PlaceholderFactory<IShipView, Guid, ShipPresenter> { }

#endregion

#region Fields

    readonly Guid _key;
    readonly IShopService _shopService;
    readonly CompositeDisposable _disposables = new();

#endregion

#region Public methods

    public ShipPresenter(IShipView view, Guid key, IShopService shopService, ItemsModel items, ProgressModel progress)
    {
        _key = key;
        _shopService = shopService;

        var ship = items.GetShip(key);

        ship.IsBought.Subscribe(isBought => view.UpdateBoughtState(isBought, ship.CurrentPrice)).AddTo(_disposables);
        ship.IsSelected.Subscribe(isSelected => view.UpdateSelectState(ship.IsBought.CurrentValue, isSelected)).AddTo(_disposables);
        ship.UpgradePrice.Subscribe(_ =>
        {
            view.SetShipStats(ship.CurrentPrice, ship.EnemyCoinBonus.CurrentValue);
            view.UpdateSolvency(progress.Diamonds.CurrentValue.IsEnough(ship.CurrentPrice));
        }).AddTo(_disposables);
        progress.Diamonds.Subscribe(diamonds => view.UpdateSolvency(diamonds.IsEnough(ship.CurrentPrice))).AddTo(_disposables);
    }

    public void BuyOrUpgradeItem() => _shopService.TryBuyOrUpgradeShip(_key);

    public void SelectItem() => _shopService.TrySelectShip(_key);

    public void Dispose() => _disposables.Dispose();

#endregion
}
}
