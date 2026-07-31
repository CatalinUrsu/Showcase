using R3;
using System;
using Zenject;
using UnityEngine;
using System.Linq;
using FMOD.Studio;
using Helpers.Audio;
using Source.Gameplay;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Data;

namespace Source.Player
{
public class PlayerFacadeGameplay : PlayerFacade
{
#region Fields

    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] PlayerWeapons _playerWeapons;
    [SerializeField] PlayerParallaxEffect _playerParallaxEffect;

    [Space] 
    [SerializeField] Collider2D _collider;
    [SerializeField] GameObject _deathEffect;

    [Inject] IGameplayMediator _gameplayMediator;
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

    public void Init(GameObject playerInputHandler)
    {
        base.Init();
        SessionService.Current.Progress.Lvl.Skip(1).Subscribe(_ => EnableShield().Forget()).AddTo(this);

        _shieldCTS = new CancellationTokenSource();
        _audioService.FlyInstance.start();
        _playerAppearance.Init();
        _playerEmergence.Init(_rb);
        _playerMovement.Init(_rb, _audioService.FlyInstance, playerInputHandler);
        InitWeapons();
    }

    public override void Deinit()
    {
        base.Deinit();
        _playerWeapons.Deinit();
        _playerAppearance.Deinit();
        _playerParallaxEffect.Deinit();

        CancelShieldCTS();
    }

    public override async UniTask ShowPlayer()
    {
        _playerEmergence.Init(_rb);
        await UniTask.Yield();

        _playerAppearance.ToggleAppearance(true);
        _playerMovement.SetOnSpawn();
        EnableShield().Forget();
        await _playerEmergence.ShowPlayer(_rb);

        _playerParallaxEffect.EnableParalax();
    }

    public override void ToggleControl(bool enable)
    {
        _playerMovement.ControllIsEnable = enable;
        _playerWeapons.ShootIsEnable = enable;
    }

#endregion

#region Private methods

    void InitWeapons()
    {
        var usedWeaponIdx = SessionService.Current.Progress.UsedWeaponIdx.Value;
        var firePower = SessionService.Current.Items.Weapons.ElementAt(usedWeaponIdx).Value.FirePower.Value;
        var fireRate = SessionService.Current.Items.Weapons.ElementAt(usedWeaponIdx).Value.FireRate.Value;
        _playerWeapons.Init(firePower, fireRate);
    }

    void DestroyPlayer()
    {
        _collider.enabled = false;
        _playerParallaxEffect.DisableParallax();
        _playerAppearance.ToggleAppearance(false);
        _playerMovement.SetOnDespawn();
        ToggleControl(false);
        PlayDeathEffects();
    }

    void PlayDeathEffects()
    {
        Instantiate(_deathEffect, transform.position, Quaternion.identity);

        FmodEventsSo.Instance.PlayerDeath.PlayOneShot();
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
            await _playerAppearance.AnimateShield(_shieldCTS.Token);
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