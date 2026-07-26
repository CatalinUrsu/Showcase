using System;
using Zenject;
using FMODUnity;
using IdleNumbers;
using UnityEngine;
using FMOD.Studio;
using Helpers.PoolSystem;
using Source.Data;

namespace Source.Gameplay
{
public class EnemyFacade : PooledObject
{
#region Fields

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] LayerMask _wallHitLayer;

    [Space] 
    [SerializeField] EnemyHitTrigger _hitTrigger;
    [SerializeField] EnemyMovement _enemyMovement;
    [SerializeField] EnemyAppearence _enemyAppearence;
    [SerializeField] EnemyHealth _enemyHealth;
    
    EnemySO _enemyDataSO;
    EnemyInitConfig _initConfig;
    GameRunModel _gameRunModel;

#endregion

#region Public methods

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer | _wallHitLayer.value) == _wallHitLayer.value)
            OnReleaseToPool_raise();
    }

    void OnDestroy() => _enemyMovement.Deinit();

    [Inject]
    public void Construct(GameRunModel gameRunModel) => _gameRunModel = gameRunModel;

    public override PooledObject Init(Action<PooledObject> onReleaseToPool, object config = null)
    {
        if (config is EnemyInitConfig soundPools) 
            _initConfig = soundPools;
            
        _hitTrigger.OnHit += OnHit_handler;
        _hitTrigger.OnHitPlayer += DestroyEnemy;
        _enemyMovement.Init(_spriteRenderer.transform);
        
        return base.Init(onReleaseToPool, config);
    }

    public override void Set(object config = null)
    {
        if (config is EnemySO enemySo)
        {
            _enemyDataSO = enemySo;
            _enemyAppearence.Set(_spriteRenderer, _enemyDataSO.EnemyAppearence);
            _enemyMovement.Set(enemySo.SpeedRadius);
            _enemyHealth.Set(enemySo.HP + SessionService.Current.Progress.Lvl.Value * 1.5f);
        }
    }

#endregion

#region Private methods

    void OnHit_handler(IdleNumber damage, Vector2 impulseDirection)
    {
        _enemyHealth.OnHit_handler(damage);
        PlayAudio(_initConfig.HitSoundPool.Get());

        if (_enemyHealth.IsDead)
            KillByBullet();
        else
        {
            _enemyMovement.OnHit_handler(impulseDirection);
            PlayFx(_initConfig.HitFxPool.Get());
        }
    }

    void KillByBullet()
    {
        DestroyEnemy();
        _gameRunModel.AddKillReward(_enemyDataSO.RewardPoints, _enemyDataSO.Coin);
    }

    void DestroyEnemy()
    {
        _enemyMovement.OnDie_handler();

        PlayFx(_initConfig.DeathFxPool.Get());
        PlayAudio(_initConfig.DeathSoundPool.Get());
        OnReleaseToPool_raise();   
    }

    void PlayAudio(EventInstance eventInstance)
    {
        eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        eventInstance.start();
    }
    
    void PlayFx(PooledObject fx)
    {
        fx.transform.position = transform.position;
        fx.Set();
    }

#endregion
}
}