using R3;
using System;
using Zenject;
using IdleNumbers;

namespace Source.Data
{
public class ShipPresenter : ItemPresenterBase
{
#region External Types

    public class Factory : PlaceholderFactory<IShipView, Guid, ShipPresenter> { }

#endregion
    
#region Fields

    readonly IShipView _view;
    readonly ShipModel _shipModel;

#endregion

#region Public methods

    public ShipPresenter(IShipView view,
                         Guid key,
                         ISessionService sessionService,
                         ItemsModel items,
                         ProgressModel progress,
                         GameBalanceSO balance,
                         FmodEventsSo fmodEventsSo) :
        base(key, sessionService, items, progress, balance, fmodEventsSo)
    {
        _view = view;
        _shipModel = _items.GetShip(key);
        _idx = _items.GetShipIdx(_key);
        _itemModel = _shipModel;

        // Subscribe to Properties value changing
        _shipModel.IsBought.Subscribe(isBought => _view.UpdateBoughtState(isBought, GetPrice())).AddTo(_disposables);
        _shipModel.IsSelected.Subscribe(isSelect => _view.UpdateSelectState(_shipModel.IsBought.CurrentValue, isSelect)).AddTo(_disposables);
        _shipModel.UpgradePrice.Subscribe(_ => SetShipStats()).AddTo(_disposables);

        _progress.Diamonds.Subscribe(_ => _view.UpdateSolvency(HasEnoughCurrency())).AddTo(_disposables);
        _progress.UsedShipIdx.Subscribe(DeselectOnSelectOtherItem).AddTo(_disposables);
    }

    public override void SelectItem()
    {
        if (!CanSelectItem()) return;

        _shipModel.Select();
        _progress.SetUsedShipIdx(_idx);

        base.SelectItem();
    }

#endregion

#region Private methods

    protected override void BuyItem()
    {
        if (!_progress.TrySpendDiamonds(_shipModel.BuyPrice.CurrentValue)) return;
        _shipModel.Buy();

        base.BuyItem();
        SelectItem();
    }

    protected override void UpgradeItem()
    {
        if (!_progress.TrySpendDiamonds(_shipModel.UpgradePrice.CurrentValue)) return;
        _shipModel.Upgrade(_balance);

        base.UpgradeItem();
    }

    protected override void DeselectOnSelectOtherItem(int selectedItemIdx)
    {
        if (selectedItemIdx == _idx || !_itemModel.IsSelected.CurrentValue) return;

        _shipModel.Deselect();
    }

    protected override bool HasEnoughCurrency() => _progress.Diamonds.CurrentValue.IsEnough(GetPrice());
    
    void SetShipStats()
    {
        _view.SetShipStats(GetPrice(), _shipModel.EnemyCoinBonus.CurrentValue);
        _view.UpdateSolvency(HasEnoughCurrency());
    }

#endregion
}
}