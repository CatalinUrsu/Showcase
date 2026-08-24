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
    readonly IShipModel _shipModel;

#endregion

#region Public methods

    public ShipPresenter(IShipView view,
                         Guid key,
                         ISessionService sessionService,
                         IItemsModelController itemsModelController,
                         IProgressModelController progressModelController,
                         FmodEventsSo fmodEventsSo) :
        base(key, sessionService, itemsModelController, progressModelController, fmodEventsSo)
    {
        _view = view;
        _shipModel = _itemsController.GetShipModel(key);
        _idx = _itemsController.GetShipIdx(_key);
        _itemModel = _shipModel;

        // Subscribe to Properties value changing
        _shipModel.IsBoughtRef.Subscribe(isBought => _view.UpdateBoughtState(isBought, GetPrice())).AddTo(_disposables);
        _shipModel.IsSelectedRef.Subscribe(isSelect => _view.UpdateSelectState(_shipModel.IsBoughtRef.CurrentValue, isSelect)).AddTo(_disposables);
        _shipModel.UpgradePriceRef.Subscribe(_ => _view.SetShipStats(GetPrice(), _shipModel.EnemyCoinBonusRef.CurrentValue)).AddTo(_disposables);

        _progressModel.DiamondsRef.Subscribe(_ => _view.UpdateSolvency(HasEnoughCurrency())).AddTo(_disposables);
        _progressModel.UsedShipIdxRef.Subscribe(DeselectOnSelectOtherItem).AddTo(_disposables);
    }

    public override void SelectItem()
    {
        if (!CanSelectItem()) return;

        _itemsController.SelectShip(_key);
        _progressController.SetUsedShipIdx(_idx);

        base.SelectItem();
    }

#endregion

#region Private methods

    protected override void BuyItem()
    {
        _itemsController.BuyShip(_key);
        _progressController.SpendDiamonds(_shipModel.BuyPriceRef.CurrentValue);

        base.BuyItem();
        SelectItem();
    }

    protected override void UpgradeItem()
    {
        _itemsController.UpdateShip(_key);
        _progressController.SpendDiamonds(_shipModel.UpgradePriceRef.CurrentValue);

        base.UpgradeItem();
    }

    protected override void DeselectOnSelectOtherItem(int selectedItemIdx)
    {
        if (selectedItemIdx == _idx || !_itemModel.IsSelectedRef.CurrentValue) return;

        _itemsController.DeselectShip(_key);
    }

    protected override bool HasEnoughCurrency() => _progressModel.DiamondsRef.CurrentValue.IsEnough(GetPrice());

#endregion
}
}