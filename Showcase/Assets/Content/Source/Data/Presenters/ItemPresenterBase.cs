using R3;
using System;
using IdleNumbers;
using Helpers.Audio;

namespace Source.Data
{
public abstract class ItemPresenterBase : IPresenterItemBase
{
#region Fields

    protected int _idx;
    protected readonly Guid _key;
    protected readonly IProgressModelController _progressController;
    protected readonly IItemsModelController _itemsController;
    protected readonly IProgressModel _progressModel;
    protected readonly CompositeDisposable _disposables = new();
    protected IItemModel _itemModel;
    
    readonly ISessionService _sessionService;
    readonly FmodEventsSo _fmodEventsSo;

#endregion

#region Public methods

    protected ItemPresenterBase(Guid key,
                                ISessionService sessionService,
                                IItemsModelController itemsModelController,
                                IProgressModelController progressModelController,
                                FmodEventsSo fmodEventsSo)
    {
        _key = key;
        _fmodEventsSo = fmodEventsSo;
        _sessionService = sessionService;

        _itemsController = itemsModelController;
        _progressController = progressModelController;

        _progressModel = _progressController.IModel;
    }

    public void BuyOrUpgradeItem()
    {
        if (!HasEnoughCurrency()) return;

        if (!_itemModel.IsBoughtRef.CurrentValue)
            BuyItem();
        else
            UpgradeItem();
    }

    public virtual void SelectItem()
    {
        _fmodEventsSo.SelectItem.PlayOneShot();
        SaveProgress();
    }

    public void Dispose() => _disposables.Dispose();

#endregion

#region Private methods

    protected virtual void BuyItem()
    {
        _fmodEventsSo.BtnBuy.PlayOneShot();
        SaveProgress();
    }

    protected virtual void UpgradeItem()
    {
        _fmodEventsSo.BtnBuy.PlayOneShot();
        SaveProgress();
    }
    
    protected abstract void DeselectOnSelectOtherItem(int selectedItemIdx);

    protected bool CanSelectItem() => _itemModel.IsBoughtRef.CurrentValue && !_itemModel.IsSelectedRef.CurrentValue;

    protected bool HasEnoughCurrency() => _progressModel.CoinsRef.CurrentValue.IsEnough(GetPrice());

    protected IdleNumber GetPrice() => _itemModel.IsBoughtRef.CurrentValue ? _itemModel.UpgradePriceRef.CurrentValue : _itemModel.BuyPriceRef.CurrentValue;

    void SaveProgress()
    {
        _sessionService.Save(ESaveFileType.Items);
        _sessionService.Save(ESaveFileType.Progress);
    }

#endregion
}
}