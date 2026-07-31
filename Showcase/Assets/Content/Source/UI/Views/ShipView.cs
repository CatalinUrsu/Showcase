using Zenject;
using IdleNumbers;
using Source.Data;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Source.UI
{
public class ShipView : ItemView, IShipView
{
#region Fields

    [SerializeField] LocalizeStringEvent _localizeStringEvent;
    [SerializeField] Color _bonusColor;
    
    [Inject] ShipPresenter.Factory _shipPresenterFactory;
    
#endregion

#region Public methods

    public override void UpdateBoughtState(bool isBought, IdleNumber price)
    {
        base.UpdateBoughtState(isBought, price);

        _txtPrice.SetText(price.AsString() + ConstSpriteAssets.SPRITE_TEXT_DIAMOND);
    }

    public void SetShipStats(IdleNumber upgradePrice, float bonus)
    {
        _txtPrice.SetText(upgradePrice.AsString() + ConstSpriteAssets.SPRITE_TEXT_DIAMOND);
        (_localizeStringEvent.StringReference["0"] as StringVariable)!.Value = bonus.ToString();
        (_localizeStringEvent.StringReference["1"] as StringVariable)!.Value = ColorUtility.ToHtmlStringRGBA(_bonusColor);
    }

    protected override void InitPresenter() => _presenterItem = _shipPresenterFactory.Create(this, _id);
    protected override void OnBuyClick_handler() => _presenterItem.BuyOrUpgradeItem();
    protected override void OnSelect_handler() => _presenterItem.SelectItem();

#endregion
}
}