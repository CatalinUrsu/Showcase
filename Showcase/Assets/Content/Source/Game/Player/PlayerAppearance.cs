using R3;
using System;
using Zenject;
using System.Linq;
using Source.Data;
using UnityEngine;

namespace Source.Game.Player
{
public class PlayerAppearance : MonoBehaviour
{
    [SerializeField] ItemLookSO[] _shipsSkins;
    [SerializeField] ItemLookSO[] _weaponsSkins;

    [Space]
    [SerializeField] SpriteRenderer _imgShip;
    [SerializeField] SpriteRenderer _imgWeaponL;
    [SerializeField] SpriteRenderer _imgWeaponR;

    [Inject] ProgressModel _progress;

    public void Init()
    {
        _progress.UsedShipId.Subscribe(SetShipSprite).AddTo(gameObject);
        _progress.UsedWeaponId.Subscribe(SetWeaponSprite).AddTo(gameObject);
    }

    public void ToggleAppearance(bool enable)
    {
        _imgShip.enabled = enable;
        _imgWeaponL.enabled = enable;
        _imgWeaponR.enabled = enable;
    }

    void SetShipSprite(Guid shipId) => _imgShip.sprite = GetSkin(_shipsSkins, shipId).ItemSprite;

    void SetWeaponSprite(Guid weaponId)
    {
        var sprite = GetSkin(_weaponsSkins, weaponId).ItemSprite;
        _imgWeaponL.sprite = sprite;
        _imgWeaponR.sprite = sprite;
    }

    // Guid.Empty (nothing selected yet) -> first skin, same as ItemsModel
    static ItemLookSO GetSkin(ItemLookSO[] skins, Guid id) => skins.FirstOrDefault(skin => skin.IdSo.Guid == id) ?? skins[0];
}
}