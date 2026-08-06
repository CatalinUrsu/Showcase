using R3;
using Zenject;
using Source.Data;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Game.Player
{
public class PlayerAppearanceBase : MonoBehaviour
{
#region Fields

    [SerializeField] protected ItemLookSO[] _shipsSkins;
    [SerializeField] protected ItemLookSO[] _weaponsSkins;
    [SerializeField] protected SpriteRenderer _imgShip;
    [SerializeField] protected SpriteRenderer _imgWeaponL;
    [SerializeField] protected SpriteRenderer _imgWeaponR;

    [Inject] IProgressModelController _progressModelController;
    
#endregion

    public virtual void Init()
    {
        _progressModelController.IModel.UsedShipIdxRef.Subscribe(SetShipSprite).AddTo(gameObject);
        _progressModelController.IModel.UsedWeaponIdxRef.Subscribe(SetWeaponSprite).AddTo(gameObject);
    }
    
    public virtual void Deinit() {}

    public virtual void ToggleAppearance(bool enable) { }

    public virtual UniTask AnimateShield(CancellationToken token) => UniTask.CompletedTask;

    void SetShipSprite(int shipIdx) => _imgShip.sprite = _shipsSkins[shipIdx].ItemSprite;

    void SetWeaponSprite(int weaponIdx)
    {
        _imgWeaponL.sprite = _weaponsSkins[weaponIdx].ItemSprite;
        _imgWeaponR.sprite = _weaponsSkins[weaponIdx].ItemSprite;
    }
}
}