using FMOD.Studio;
using UnityEngine;
using Source.Data;
using Helpers.Audio;
using Helpers.PoolSystem;

namespace Source.Game.Gameplay
{
public class EnemiesInitConfig
{
    public Pool<EventInstance> HitSfxPool { get; private set; }
    public Pool<EventInstance> DeathSfxPool { get; private set; }
    public Pool<PooledObject> HitFxPool { get; private set; }
    public Pool<PooledObject> DeathFxPool { get; private set; }

    public EnemiesInitConfig(PooledObject hitFxPrefab,
                             PooledObject deathFxPrefab,
                             Transform poolActive,
                             Transform poolInactive,
                             FmodEventsSo fmodEventsSo)
    {
        HitSfxPool = new FactoryFmodEvents.Builder(fmodEventsSo.Hit)
                       .SetPreloadCount(5)
                       .SetMaxCount(7)
                       .Set3DAttributes(true)
                       .Build();

        DeathSfxPool = new FactoryFmodEvents.Builder(fmodEventsSo.EnemyDeath)
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
        HitSfxPool.Clear();
        DeathSfxPool.Clear();
        HitFxPool.Clear();
        DeathFxPool.Clear();
    }
}
}