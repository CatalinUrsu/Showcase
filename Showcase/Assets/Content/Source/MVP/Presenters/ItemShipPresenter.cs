using R3;
using UnityEngine;
using IdleNumbers;
using System.Linq;
using Source.Audio;
using Helpers.Audio;
using Source.Session;

namespace Source.MVP
{
public class ItemShipPresenter : IPresenterItemBase
{
#region Fields

    IViewItem _itemView;
    IViewItemShip _shipView;
    ItemModelShip _shipModel;
    int _itemIdx;

#endregion

#region Public methods

    public ItemShipPresenter(GameObject shipGO, IViewItem itemView, IViewItemShip shipView, int itemIdx)
    {
        _itemView = itemView;
        _shipView = shipView;
        _shipModel = SessionManager.Current.Items.Ships.ElementAt(itemIdx).Value;
        _itemIdx = itemIdx;

        SubscribeProperties(shipGO);
    }

    public void BuyOrUpgradeItem()
    {
        if (!SessionManager.Current.Progress.Diamonds.Value.IsEnough(GetPrice())) return;
        
        FmodEvents.Instance.BtnBuy.PlayOneShot();

        if (!_shipModel.IsBought.Value)
            BuyItem();
        else
            UpgradeItem();
    }

    public void SelectItem()
    {
        if (!_shipModel.IsBought.Value || _shipModel.IsSelected.Value) return;
     
        FmodEvents.Instance.BtnSelect.PlayOneShot();
        
        _shipModel.IsSelected.Value = true;
        SessionManager.Current.Progress.UsedShipIdx.Value = _itemIdx;
        SessionManager.Current.Save(ESaveFileType.Items);
        SessionManager.Current.Save(ESaveFileType.Progress);
    }

#endregion

#region Private methods

    void SubscribeProperties(GameObject itemGO)
    {
        _shipModel.IsBought.Subscribe(isBought => _itemView.OnChangeBoughtState_handler(isBought, GetPrice())).AddTo(itemGO);
        _shipModel.IsSelected.Subscribe(isSelect => _itemView.OnChangeSelectState_handler(_shipModel.IsBought.Value, isSelect)).AddTo(itemGO);
        _shipModel.UpgradePrice.Subscribe(_ => _shipView.OnUpgrade_handler(GetPrice(), _shipModel.EnemyCoinBonus.Value)).AddTo(itemGO);
        SessionManager.Current.Progress.Diamonds.Subscribe(_ => _itemView.OnUpdateSolvency_handler(SessionManager.Current.Progress.Diamonds.Value.IsEnough(GetPrice()))).AddTo(itemGO);
        SessionManager.Current.Progress.UsedShipIdx.Subscribe(DeselectOnSelectOtherItem).AddTo(itemGO);
    }

    void BuyItem()
    {
        SessionManager.Current.Progress.Diamonds.Value -= _shipModel.BuyPrice.Value;
        _shipModel.IsBought.Value = true;
        SelectItem();
    }

    void UpgradeItem()
    {
        var diamondsToSpend = _shipModel.UpgradePrice.Value;
        _shipModel.EnemyCoinBonus.Value += ConstUpgradeItems.SHIP_BONUS_INCREASE;
        _shipModel.UpgradePrice.Value *= ConstUpgradeItems.SHIP_PRICE_MULTIPLIER;

        SessionManager.Current.Progress.Diamonds.Value -= diamondsToSpend;
        SessionManager.Current.Save(ESaveFileType.Items);
        SessionManager.Current.Save(ESaveFileType.Progress);
    }

    void DeselectOnSelectOtherItem(int selectedItemIdx)
    {
        if (selectedItemIdx == _itemIdx) return;

        _shipModel.IsSelected.Value = false;
    }

    IdleNumber GetPrice() => _shipModel.IsBought.Value ? _shipModel.UpgradePrice.Value : _shipModel.BuyPrice.Value;

#endregion
}
}