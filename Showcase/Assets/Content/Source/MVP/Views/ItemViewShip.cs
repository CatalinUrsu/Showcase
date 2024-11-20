using IdleNumbers;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Source.MVP
{
public class ItemViewShip : ItemView, IViewItemShip
{
#region Fields

    [SerializeField] LocalizeStringEvent _localizeStringEvent;
    [SerializeField] Color _bonusColor;

#endregion

#region Public methods

    public override void OnChangeBoughtState_handler(bool isBought, IdleNumber price)
    {
        base.OnChangeBoughtState_handler(isBought, price);

        _txtPrice.SetText(price.AsString() + ConstSpriteAssets.SPRITE_TEXT_DIAMOND);
    }

    public void OnUpgrade_handler(IdleNumber upgradePrice, float bonus)
    {
        _txtPrice.SetText(upgradePrice.AsString() + ConstSpriteAssets.SPRITE_TEXT_DIAMOND);
        (_localizeStringEvent.StringReference["0"] as StringVariable)!.Value = bonus.ToString();
        (_localizeStringEvent.StringReference["1"] as StringVariable)!.Value = ColorUtility.ToHtmlStringRGBA(_bonusColor);
    }

    protected override void SetItemInfo() => _presenterItem = new ItemShipPresenter(gameObject, this, this, _itemIdx);
    protected override void OnBuyClick_handler() => _presenterItem.BuyOrUpgradeItem();
    protected override void OnSelect_handler() => _presenterItem.SelectItem();

#endregion
}
}