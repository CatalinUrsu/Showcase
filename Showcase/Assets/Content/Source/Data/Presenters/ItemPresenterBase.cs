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
    protected readonly ProgressModel _progress;
    protected readonly ItemsModel _items;
    protected readonly GameBalanceSO _balance;
    protected readonly CompositeDisposable _disposables = new();
    protected ItemModel _itemModel;

    readonly ISessionService _sessionService;
    readonly FmodEventsSo _fmodEventsSo;

#endregion

#region Public methods

    protected ItemPresenterBase(Guid key,
                                ISessionService sessionService,
                                ItemsModel items,
                                ProgressModel progress,
                                GameBalanceSO balance,
                                FmodEventsSo fmodEventsSo)
    {
        _key = key;
        _fmodEventsSo = fmodEventsSo;
        _sessionService = sessionService;
        _items = items;
        _progress = progress;
        _balance = balance;
    }

    public void BuyOrUpgradeItem()
    {
        if (!_itemModel.IsBought.CurrentValue)
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

    protected bool CanSelectItem() => _itemModel.IsBought.CurrentValue && !_itemModel.IsSelected.CurrentValue;

    protected virtual bool HasEnoughCurrency() => _progress.Coins.CurrentValue.IsEnough(GetPrice());

    protected IdleNumber GetPrice() => _itemModel.CurrentPrice;

    protected void SaveProgress()
    {
        _sessionService.Save(ESaveFileType.Items);
        _sessionService.Save(ESaveFileType.Progress);
    }

#endregion
}
}
