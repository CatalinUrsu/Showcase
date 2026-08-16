using System;
using Zenject;
using FMODUnity;
using FMOD.Studio;
using IdleNumbers;
using Source.Data;
using UnityEngine;
using Helpers.PoolSystem;

namespace Source.Game.Gameplay
{
[RequireComponent(typeof(EnemyHitTrigger), typeof(EnemyMovement), typeof(EnemyAppearence))]
[RequireComponent(typeof(EnemyHealth))]
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
    EnemiesInitConfig _initConfig;
    IGameRunModelController _runModelController;
    IProgressModel _progressModel;

#endregion

#region Monobeh

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer | _wallHitLayer.value) == _wallHitLayer.value)
            OnReleaseToPool_raise();
    }

    void OnDestroy() => _enemyMovement.Deinit();

#endregion
    
#region Public methods
    
    [Inject]
    public void Construct(IGameRunModelController runModelController, IProgressModelController progressModelController)
    {
        _runModelController = runModelController;
        _progressModel = progressModelController.IModel;
    }

    public override PooledObject Init(Action<PooledObject> onReleaseToPool, object config = null)
    {
        if (config is EnemiesInitConfig initConfig) 
            _initConfig = initConfig;
            
        _hitTrigger.OnHit += OnHit_handler;
        _hitTrigger.OnHitPlayer += DestroyEnemy;
        _enemyMovement.Init(_spriteRenderer.transform);
        
        return base.Init(onReleaseToPool, config);
    }

    public override void Set(object config = null)
    {
        if (config is not EnemySO enemySo) return;
        
        _enemyDataSO = enemySo;
        _enemyAppearence.Set(_spriteRenderer, _enemyDataSO.EnemyAppearance);
        _enemyMovement.Set(enemySo.SpeedRadius);
        _enemyHealth.Set(enemySo.HP, _progressModel.LvlRef.CurrentValue);
    }

#endregion

#region Private methods

    void OnHit_handler(IdleNumber damage, Vector2 impulseDirection)
    {
        _enemyHealth.TakeDamage(damage);
        PlayAudio(_initConfig.HitSfxPool.Get());

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
        _runModelController.AddCoinsReward(_enemyDataSO.RewardPoints, _enemyDataSO.Coin);
    }

    void DestroyEnemy()
    {
        _enemyMovement.OnDie_handler();

        PlayFx(_initConfig.DeathFxPool.Get());
        PlayAudio(_initConfig.DeathSfxPool.Get());
        OnReleaseToPool_raise();   
    }

    void PlayAudio(EventInstance eventInstance)
    {
        eventInstance.set3DAttributes(transform.position.To3DAttributes());
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