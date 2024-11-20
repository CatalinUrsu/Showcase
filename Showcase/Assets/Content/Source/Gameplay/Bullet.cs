using System;
using IdleNumbers;
using UnityEngine;
using Helpers.PoolSystem;

namespace Source.Gameplay
{
public class Bullet : PooledObject
{
    [SerializeField] float _speed;
    [SerializeField] Rigidbody2D _rb;
    
    IdleNumber _hitDamage;

    public override void Init(Action<PooledObject> onReleaseToPool, object config)
    {
        base.Init(onReleaseToPool, config);

        if (config is IdleNumber hitDamage) 
            _hitDamage = hitDamage;
    }

    public override void Set(object config = null)
    {
        _rb.linearVelocity = Vector2.up * _speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyHitTrigger enemyCollider))
        {
            Vector2 impulseDirection = other.transform.position - transform.position;
            enemyCollider.OnHit_raise(_hitDamage, impulseDirection);
        }

        OnReleaseToPool_raise();
    }
}
}