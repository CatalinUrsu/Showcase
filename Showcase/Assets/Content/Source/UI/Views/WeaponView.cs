using TMPro;
using UnityEngine;
using Source.Data;
using IdleNumbers;
using Helpers.Audio;

namespace Source.UI
{
public class WeaponView : ItemView, IWeaponView
{
#region Fields

    [SerializeField] TextMeshProUGUI _txtFirePower;
    [SerializeField] TextMeshProUGUI _txtFireRate;

#endregion

#region Public methods

    public override void UpdateBoughtState(bool isBought, IdleNumber price)
    {
        base.UpdateBoughtState(isBought, price);

        _txtPrice.SetText($"{price.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}");
    }

    public void SetWeaponsStats(IdleNumber upgradePrice, IdleNumber firePower, float fireRate)
    {
        _fmodEvents.BtnBuy.PlayOneShot();

        _txtPrice.SetText($"{upgradePrice.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}");
        _txtFirePower.SetText($"{firePower.AsString()} {ConstSpriteAssets.SPRITE_TEXT_FIRE_POWER}");
        _txtFireRate.SetText($"{fireRate:F2} {ConstSpriteAssets.SPRITE_TEXT_FIRE_RATE}");
    }

    protected override void SetItemInfo() => _presenterItem = new WeaponPresenter(this, _sessionService, _itemsModelController, _progressModelController, _fmodEvents, _id);
    protected override void OnBuyClick_handler() => _presenterItem.BuyOrUpgradeItem();
    protected override void OnSelect_handler() => _presenterItem.SelectItem();

#endregion
}
}