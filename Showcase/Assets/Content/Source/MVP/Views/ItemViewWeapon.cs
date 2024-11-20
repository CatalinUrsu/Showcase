using TMPro;
using IdleNumbers;
using UnityEngine;

namespace Source.MVP
{
public class ItemViewWeapon : ItemView, IViewItemWeapon
{
#region Fields

    [SerializeField] TextMeshProUGUI _txtFirePower;
    [SerializeField] TextMeshProUGUI _txtFireRate;

#endregion

#region Public methods

    public override void OnChangeBoughtState_handler(bool isBought, IdleNumber price)
    {
        base.OnChangeBoughtState_handler(isBought, price);

        _txtPrice.SetText($"{price.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}");
    }

    public void OnUpgrade_handler(IdleNumber upgradePrice, IdleNumber firePower, float fireRate)
    {
        _txtPrice.SetText($"{upgradePrice.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}");
        _txtFirePower.SetText($"{firePower.AsString()} {ConstSpriteAssets.SPRITE_TEXT_FIRE_POWER}");
        _txtFireRate.SetText($"{fireRate.ToString("F2")} {ConstSpriteAssets.SPRITE_TEXT_FIRE_RATE}");
    }

    protected override void SetItemInfo() => _presenterItem = new ItemWeaponPresenter(gameObject, this, this, _itemIdx);
    protected override void OnBuyClick_handler() => _presenterItem.BuyOrUpgradeItem();
    protected override void OnSelect_handler() => _presenterItem.SelectItem();

#endregion
}
}