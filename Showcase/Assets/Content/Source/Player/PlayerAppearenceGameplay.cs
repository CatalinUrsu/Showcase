using Helpers;
using DG.Tweening;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Player
{
public class PlayerAppearenceGameplay : PlayerAppearenceBase
{
#region Fields

    [Header("Shield elements")] 
    [SerializeField] SpriteRenderer _shieldInSprite;
    [SerializeField] SpriteRenderer _shieldOutSprite;
    [SerializeField] float _invincibilityDuration;
    [SerializeField] AnimationCurve _invincibilityCurve;

    Transform _shieldInTransform;
    Transform _shieldOutTransform;
    Vector3 _shieldAnimRotation = new(0, 0, 1000);
    Sequence _shieldSequence;

#endregion

#region Public methods

    public override void Init()
    {
        base.Init();

        _shieldInTransform = _shieldInSprite.transform;
        _shieldOutTransform = _shieldOutSprite.transform;
    }

    public void Deinit() => _shieldSequence.CheckAndEnd(false);

    public async UniTask AnimateShield(CancellationToken token)
    {
        _shieldSequence.CheckAndEnd();
        _shieldInTransform.gameObject.SetActive(true);
        _shieldOutTransform.gameObject.SetActive(true);
        
        _shieldSequence = DOTween.Sequence()
                                 .Pause()
                                 .Join(_shieldInSprite.DOFade(1, _invincibilityDuration)
                                                      .From(0)
                                                      .SetEase(_invincibilityCurve))
                                 .Join(_shieldOutSprite.DOFade(1, _invincibilityDuration)
                                                       .From(0)
                                                       .SetEase(_invincibilityCurve))
                                 .Join(
                                       _shieldInTransform.DORotate(_shieldAnimRotation, _invincibilityDuration, RotateMode.FastBeyond360)
                                                         .SetRelative(true)
                                                         .SetEase(Ease.Linear))
                                 .Join(
                                       _shieldOutTransform.DORotate(-_shieldAnimRotation, _invincibilityDuration, RotateMode.FastBeyond360)
                                                          .SetRelative(true)
                                                          .SetEase(Ease.Linear))
                                 .OnComplete(() =>
                                 {
                                     _shieldInTransform.gameObject.SetActive(false);
                                     _shieldOutTransform.gameObject.SetActive(false);
                                 });

            await _shieldSequence.Play().ToUniTask().AttachExternalCancellation(token);
    }

    public void ToggleAppearence(bool enable)
    {
        _imgShip.enabled = enable;
        _imgWeaponL.enabled = enable;
        _imgWeaponR.enabled = enable;
    }

#endregion
}
}