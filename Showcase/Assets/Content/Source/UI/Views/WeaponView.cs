using TMPro;
using Zenject;
using UnityEngine;
using Source.Data;
using IdleNumbers;

namespace Source.UI
{
public class WeaponView : ItemView, IWeaponView
{
#region Fields

    [SerializeField] TextMeshProUGUI _txtFirePower;
    [SerializeField] TextMeshProUGUI _txtFireRate;

    [Inject] WeaponPresenter.Factory _weaponPresenterFactory;
    
#endregion

#region Public methods

    public override void UpdateBoughtState(bool isBought, IdleNumber price)
    {
        base.UpdateBoughtState(isBought, price);

        _txtPrice.SetText($"{price.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}");
    }

    public void SetWeaponsStats(IdleNumber upgradePrice, IdleNumber firePower, float fireRate)
    {
        _txtPrice.SetText($"{upgradePrice.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}");
        _txtFirePower.SetText($"{firePower.AsString()} {ConstSpriteAssets.SPRITE_TEXT_FIRE_POWER}");
        _txtFireRate.SetText($"{fireRate:F2} {ConstSpriteAssets.SPRITE_TEXT_FIRE_RATE}");
    }

    protected override void InitPresenter() => _presenterItem = _weaponPresenterFactory.Create(this, _id);
    protected override void OnBuyClick_handler() => _presenterItem.BuyOrUpgradeItem();
    protected override void OnSelect_handler() => _presenterItem.SelectItem();

#endregion
}
}