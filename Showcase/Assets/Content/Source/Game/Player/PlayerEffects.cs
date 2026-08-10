using Zenject;
using Helpers;
using FMODUnity;
using DG.Tweening;
using UnityEngine;
using Source.Data;
using Helpers.Audio;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Game.Player
{
public class PlayerEffects : MonoBehaviour
{
#region Fields

    [Header("Shield elements")] 
    [SerializeField] SpriteRenderer _shieldInSprite;
    [SerializeField] SpriteRenderer _shieldOutSprite;
    [SerializeField] float _invincibilityDuration;
    [SerializeField] AnimationCurve _invincibilityCurve;
    
    [Space]
    [SerializeField] Collider2D _collider;
    [SerializeField] GameObject _deathEffect;

    UniTask _shieldTask;
    CancellationTokenSource _shieldCTS = new();
    
    Transform _shieldInTransform;
    Transform _shieldOutTransform;
    Vector3 _shieldAnimRotation = new(0, 0, 1000);
    Sequence _shieldSequence;
    
    EventReference _deathAudioReference;

#endregion

#region Public methods
    
    [Inject]
    public void Construct(FmodEventsSo fmodEventsSo) => _deathAudioReference = fmodEventsSo.PlayerDeath;

    public void Init()
    {
        _shieldInTransform = _shieldInSprite.transform;
        _shieldOutTransform = _shieldOutSprite.transform;
    }

    public void Deinit()
    {
        _shieldCTS.Dispose();
        _shieldSequence.CheckAndEnd(false);
    }

    public async UniTaskVoid EnableShield()
    {
        await CancelPrevShield();
        StartShieldAnim().Forget();
    }

    public void PlayDeathEffects()
    {
        _collider.enabled = false;
        _deathAudioReference.PlayOneShot();

        Instantiate(_deathEffect, transform.position, Quaternion.identity);
    }

#endregion

#region Private methods
    
    async UniTask CancelPrevShield()
    {
        // If shield animation is already running, cancel it and wait until cleanup finishes.
        if (_shieldTask.Status == UniTaskStatus.Pending)
        {
            _shieldCTS?.Cancel();
            await _shieldTask.SuppressCancellationThrow();
            _shieldCTS = new CancellationTokenSource();
        }
        else
            await UniTask.CompletedTask;
    }

    async UniTaskVoid StartShieldAnim()
    {
        _collider.enabled = false;
            
        ToggleShield(true);

        _shieldSequence.FinishAndGetNew()
                       .Join(GetShieldFade(_shieldInSprite))
                       .Join(GetShieldFade(_shieldOutSprite))
                       .Join(GetShieldRotation(_shieldInTransform, _shieldAnimRotation))
                       .Join(GetShieldRotation(_shieldOutTransform, -_shieldAnimRotation))
                       .OnComplete(() => ToggleShield(false));

        _shieldTask = _shieldSequence.ToUniTask(cancellationToken: _shieldCTS.Token);
        
        // Cancellation is expected when a new shield activation interrupts this one.
        await _shieldTask.SuppressCancellationThrow();

        if (!_shieldCTS.Token.IsCancellationRequested)
            _collider.enabled = true;
    }

    Tween GetShieldFade(SpriteRenderer shield)
    {
        return shield.DOFade(1, _invincibilityDuration)
               .From(0)
               .SetEase(_invincibilityCurve);
    }

    Tween GetShieldRotation(Transform shieldTransform, Vector3 rotation)
    {
        return shieldTransform.DORotate(rotation, _invincibilityDuration, RotateMode.FastBeyond360)
                              .SetRelative(true)
                              .SetEase(Ease.Linear);
    }

    void ToggleShield(bool enable)
    {
        _shieldInTransform.gameObject.SetActive(enable);
        _shieldOutTransform.gameObject.SetActive(enable);
    }

#endregion
}
}