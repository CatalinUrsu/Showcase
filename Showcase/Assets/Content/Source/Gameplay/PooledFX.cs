using System;
using UnityEngine;
using Helpers.PoolSystem;

namespace Source.Gameplay
{
public class PooledFX : PooledObject
{
    ParticleSystem _fx;

    public override void Init(Action<PooledObject> onReleaseToPool, object config)
    {
        base.Init(onReleaseToPool, config);
        
        _fx = GetComponent<ParticleSystem>();
        var main = _fx.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public override void Set(object config = null) => _fx.Play();

    void OnParticleSystemStopped() => OnReleaseToPool_raise();
}
}