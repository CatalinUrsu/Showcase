using Source.Session;
using Source.SO.Items;
using UniRx;
using UnityEngine;

namespace Source.Player
{
public class PlayerAppearenceBase : MonoBehaviour
{
#region Fields

    [SerializeField] protected ItemLookSO[] _shipsSkins;
    [SerializeField] protected ItemLookSO[] _weaponsSkins;
    [SerializeField] protected SpriteRenderer _imgShip;
    [SerializeField] protected SpriteRenderer _imgWeaponL;
    [SerializeField] protected SpriteRenderer _imgWeaponR;

#endregion
    
    public virtual void Init()
    {
        SessionManager.Current.Progress.UsedShipIdx.Subscribe(SetShipSprite).AddTo(gameObject);
        SessionManager.Current.Progress.UsedWeaponIdx.Subscribe(SetWeaponSprite).AddTo(gameObject);
    }
    
    void SetShipSprite(int shpiIdx)
    {
        _imgShip.sprite = _shipsSkins[shpiIdx].ItemSprite;
    }

    void SetWeaponSprite(int weaponIdx)
    {
        _imgWeaponL.sprite = _weaponsSkins[weaponIdx].ItemSprite;
        _imgWeaponR.sprite = _weaponsSkins[weaponIdx].ItemSprite;
    }
}
}