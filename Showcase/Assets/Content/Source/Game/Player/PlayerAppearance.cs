using R3;
using Zenject;
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

    [Inject] IProgressModelController _progressModelController;

    public void Init()
    {
        _progressModelController.IModel.UsedShipIdxRef.Subscribe(SetShipSprite).AddTo(gameObject);
        _progressModelController.IModel.UsedWeaponIdxRef.Subscribe(SetWeaponSprite).AddTo(gameObject);
    }

    public void ToggleAppearance(bool enable)
    {
        _imgShip.enabled = enable;
        _imgWeaponL.enabled = enable;
        _imgWeaponR.enabled = enable;
    }

    void SetShipSprite(int shipIdx) => _imgShip.sprite = _shipsSkins[shipIdx].ItemSprite;

    void SetWeaponSprite(int weaponIdx)
    {
        _imgWeaponL.sprite = _weaponsSkins[weaponIdx].ItemSprite;
        _imgWeaponR.sprite = _weaponsSkins[weaponIdx].ItemSprite;
    }
}
}