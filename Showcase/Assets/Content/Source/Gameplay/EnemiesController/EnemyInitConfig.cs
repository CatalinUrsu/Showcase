using FMOD.Studio;
using Source.Audio;
using UnityEngine;
using Helpers.PoolSystem;

namespace Source.Gameplay
{
public class EnemyInitConfig
{
    public IPool<EventInstance> HitSoundPool { get; private set; }
    public IPool<EventInstance> DeathSoundPool { get; private set; }
    public IPool<PooledObject> HitFxPool { get; private set; }
    public IPool<PooledObject> DeathFxPool { get; private set; }

    public EnemyInitConfig(PooledObject hitFxPrefab, PooledObject deathFxPrefab, Transform poolActive, Transform poolInactive)
    {
        HitSoundPool = new FactoryFmodEvents.Builder(FmodEvents.Instance.Hit)
                       .WithPreloadCount(5)
                       .WithMaxCount(7)
                       .With3DAttributes(true)
                       .Build();

        DeathSoundPool = new FactoryFmodEvents.Builder(FmodEvents.Instance.EnemyDeath)
                         .WithPreloadCount(3)
                         .WithMaxCount(5)
                         .With3DAttributes(true)
                         .Build();

        HitFxPool = new FactoryGO.Builder(hitFxPrefab)
                    .WithParents(poolActive,poolInactive)
                    .WithPreloadCount(6)
                    .WithMaxCount(10)
                    .Build();

        DeathFxPool = new FactoryGO.Builder(deathFxPrefab)
                      .WithParents(poolActive,poolInactive)
                      .WithPreloadCount(3)
                      .WithMaxCount(5)
                      .Build();
    }

    public void Deinit()
    {
        HitSoundPool.Clear();
        DeathSoundPool.Clear();
        HitFxPool.Clear();
        DeathFxPool.Clear();
    }
}
}