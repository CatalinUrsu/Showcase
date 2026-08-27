using System;
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
    [SerializeField] ParticleSystem _deathFx;

    bool _isShieldEnabled;
    Transform _shieldInTransform;
    Transform _shieldOutTransform;
    Sequence _shieldSequence;
    CancellationTokenSource _shieldCTS = new();
    readonly Vector3 _shieldAnimRotation = new(0, 0, 1000);
    
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
        _deathFx.Play(true);
    }

#endregion

#region Private methods

    async UniTask CancelPrevShield()
    {
        if (!_isShieldEnabled)
            return;

        // If shield animation is already running, cancel it and wait until cleanup finishes.
        _shieldCTS?.Cancel();
        await UniTask.WaitUntil(() => !_isShieldEnabled);
        _shieldCTS = new CancellationTokenSource();
    }

    async UniTaskVoid StartShieldAnim()
    {
        ToggleShield(true);

        _isShieldEnabled = true;
        _collider.enabled = false;
        _shieldSequence = _shieldSequence.FinishAndGetNew()
                                         .Join(GetShieldFade(_shieldInSprite))
                                         .Join(GetShieldFade(_shieldOutSprite))
                                         .Join(GetShieldRotation(_shieldInTransform, _shieldAnimRotation))
                                         .Join(GetShieldRotation(_shieldOutTransform, -_shieldAnimRotation))
                                         .OnComplete(() => ToggleShield(false));
        
        try
        {
            await _shieldSequence.ToUniTask(cancellationToken: _shieldCTS.Token);
        }
        catch (Exception e)
        {
            if (e is not OperationCanceledException)
                throw;
        }
        finally
        {
            if (this != null)
            {
                _collider.enabled = true;
                _isShieldEnabled = false;
            }
        }
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