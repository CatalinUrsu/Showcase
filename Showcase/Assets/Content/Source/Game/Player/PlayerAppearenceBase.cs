using Source.Data;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

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
        SessionService.Current.Progress.UsedShipIdx.Subscribe(SetShipSprite).AddTo(gameObject);
        SessionService.Current.Progress.UsedWeaponIdx.Subscribe(SetWeaponSprite).AddTo(gameObject);
    }
    
    public virtual void Deinit() {}

    public virtual void ToggleAppearance(bool enable) { }

    public virtual UniTask AnimateShield(CancellationToken token) => UniTask.CompletedTask;

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