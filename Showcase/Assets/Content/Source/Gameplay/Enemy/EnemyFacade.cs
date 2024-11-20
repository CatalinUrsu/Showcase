using System;
using Zenject;
using FMODUnity;
using Source.MVP;
using IdleNumbers;
using UnityEngine;
using FMOD.Studio;
using Source.Session;
using Source.SO.Enemy;
using Helpers.PoolSystem;

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
    GameProgressModel _gameProgressModel;

#endregion

#region Public methods

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer | _wallHitLayer.value) == _wallHitLayer.value)
            OnReleaseToPool_raise();
    }

    void OnDestroy() => _enemyMovement.Deinit();

    [Inject]
    public void Construct(GameProgressModel gameProgressModel) => _gameProgressModel = gameProgressModel;

    public override void Init(Action<PooledObject> onReleaseToPool, object config)
    {
        base.Init(onReleaseToPool, config);

        if (config is EnemyInitConfig soundPools) 
            _initConfig = soundPools;
            
        _hitTrigger.OnHit += OnHit_handler;
        _hitTrigger.OnHitPlayer += DestroyEnemy;
        _enemyMovement.Init(_spriteRenderer.transform);
    }

    public override void Set(object config = null)
    {
        if (config is EnemySO enemySo)
        {
            _enemyDataSO = enemySo;
            _enemyAppearence.Set(_spriteRenderer, _enemyDataSO.EnemyAppearence);
            _enemyMovement.Set(enemySo.SpeedRadius);
            _enemyHealth.Set(enemySo.HP + SessionManager.Current.Progress.Lvl.Value * 1.5f);
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
        _gameProgressModel.AddKillReward(_enemyDataSO.RewardPoints, _enemyDataSO.Coin);
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