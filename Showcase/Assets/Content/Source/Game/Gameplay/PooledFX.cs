using System;
using UnityEngine;
using Helpers.PoolSystem;

namespace Source.Game.Gameplay
{
public class PooledFX : PooledObject
{
    ParticleSystem _fx;

    public override PooledObject Init(Action<PooledObject> onReleaseToPool, object config = null)
    {
        _fx = GetComponent<ParticleSystem>();
        var main = _fx.main;
        main.stopAction = ParticleSystemStopAction.Callback;
        
        return base.Init(onReleaseToPool, config);
    }

    public override void Set(object config = null) => _fx.Play();

    void OnParticleSystemStopped() => OnReleaseToPool_raise();
}
}