using FMOD.Studio;
using UnityEngine;
using Helpers.Audio;
using Helpers.PoolSystem;
using Source.Data;

namespace Source.Gameplay
{
public class EnemyInitConfig
{
    public Pool<EventInstance> HitSoundPool { get; private set; }
    public Pool<EventInstance> DeathSoundPool { get; private set; }
    public Pool<PooledObject> HitFxPool { get; private set; }
    public Pool<PooledObject> DeathFxPool { get; private set; }

    public EnemyInitConfig(PooledObject hitFxPrefab, PooledObject deathFxPrefab, Transform poolActive, Transform poolInactive)
    {
        HitSoundPool = new FactoryFmodEvents.Builder(FmodEventsSo.Instance.Hit)
                       .SetPreloadCount(5)
                       .SetMaxCount(7)
                       .Set3DAttributes(true)
                       .Build();

        DeathSoundPool = new FactoryFmodEvents.Builder(FmodEventsSo.Instance.EnemyDeath)
                         .SetPreloadCount(3)
                         .SetMaxCount(5)
                         .Set3DAttributes(true)
                         .Build();

        HitFxPool = new Factory.Builder(hitFxPrefab)
                    .SetParents(poolActive,poolInactive)
                    .SetPreloadCount(6)
                    .SetMaxCount(10)
                    .Build();

        DeathFxPool = new Factory.Builder(deathFxPrefab)
                      .SetParents(poolActive,poolInactive)
                      .SetPreloadCount(3)
                      .SetMaxCount(5)
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