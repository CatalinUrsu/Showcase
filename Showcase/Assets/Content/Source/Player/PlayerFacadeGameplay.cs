using UniRx;
using System;
using Zenject;
using Helpers;
using UnityEngine;
using System.Linq;
using FMOD.Studio;
using Source.Audio;
using Source.Session;
using Source.Gameplay;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Player
{
public class PlayerFacadeGameplay : MonoBehaviour
{
#region Fields

    [SerializeField] PlayerEmergence _playerEmergence;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerAppearenceGameplay _playerAppearence;
    [SerializeField] PlayerWeapons _playerWeapons;
    [SerializeField] PlayerParallaxEffect _playerParallaxEffect;

    [Space] 
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Collider2D _collider;
    [SerializeField] GameObject _deathEffect;

    IGameplayMediator _gameplayMediator;
    IAudioService _audioService;
    CancellationTokenSource _shieldCTS;

#endregion

#region Public methods

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyHitTrigger enemyCollider))
        {
            DestroyPlayer();
            _gameplayMediator.SetGameState(EGameplayState.Loose);
            enemyCollider.OnHitPlayer_raise();
        }
    }

    [Inject]
    public void Construct(IGameplayMediator gameplayMediator, IAudioService audioService)
    {
        _gameplayMediator = gameplayMediator;
        _audioService = audioService;
    }

    public void Init(GameObject playerInputHandler)
    {
        SessionManager.Current.Progress.Lvl.Skip(1).Subscribe(_ => EnableShield().Forget()).AddTo(this);

        _shieldCTS = new CancellationTokenSource();
        _audioService.FlyInstance.start();
        _playerAppearence.Init();
        _playerEmergence.Init(_rb);
        _playerMovement.Init(_rb, _audioService.FlyInstance, playerInputHandler);
        InitWeapons();
    }

    public void Deinit()
    {
        _playerWeapons.Deinit();
        _playerAppearence.Deinit();
        _playerParallaxEffect.Deinit();
        _audioService.FlyInstance.stop(STOP_MODE.IMMEDIATE);

        CancelShieldCTS();
    }

    public void ToggleControl(bool enable)
    {
        _playerMovement.ControllIsEnable = enable;
        _playerWeapons.ShootIsEnable = enable;
    }

    public async UniTaskVoid ShowPlayer()
    {
        _playerEmergence.Init(_rb);
        await UniTask.Yield();

        _playerAppearence.ToggleAppearence(true);
        _playerMovement.SetOnSpawn();
        EnableShield().Forget();
        await _playerEmergence.ShowPlayer(_rb);

        _playerParallaxEffect.EnableParalax();
    }

#endregion

#region Private methods

    void InitWeapons()
    {
        var usedWeaponIdx = SessionManager.Current.Progress.UsedWeaponIdx.Value;
        var firePower = SessionManager.Current.Items.Weapons.ElementAt(usedWeaponIdx).Value.FirePower.Value;
        var fireRate = SessionManager.Current.Items.Weapons.ElementAt(usedWeaponIdx).Value.FireRate.Value;
        _playerWeapons.Init(firePower, fireRate);
    }

    void DestroyPlayer()
    {
        _collider.enabled = false;
        _playerParallaxEffect.DisableParallax();
        _playerAppearence.ToggleAppearence(false);
        _playerMovement.SetOnDespawn();
        ToggleControl(false);
        PlayDeathEffects();
    }

    void PlayDeathEffects()
    {
        Instantiate(_deathEffect, transform.position, Quaternion.identity);

        FmodEvents.Instance.PlayerDeath.PlayOneShot();
    }
    
    async UniTaskVoid EnableShield()
    {
        if (!_collider.enabled)
        {
            CancelShieldCTS();
            _shieldCTS = new CancellationTokenSource();
        }

        try
        {
            _collider.enabled = false;
            await _playerAppearence.AnimateShield(_shieldCTS.Token);
            _collider.enabled = true;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Shield animation was canceled");
        }
    }

    void CancelShieldCTS()
    {
        if (_shieldCTS == null) return;
        _shieldCTS.Cancel();
        _shieldCTS.Dispose();
    }

#endregion
}
}