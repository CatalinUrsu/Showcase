using R3;
using System;
using IdleNumbers;
using UnityEngine;
using FMOD.Studio;
using Source.Audio;
using Source.Gameplay;
using Helpers.PoolSystem;

namespace Source.Player
{
public class PlayerWeapons : MonoBehaviour
{
#region Fields

    [SerializeField] Bullet _bullet;
    [SerializeField] PooledFX _shootFx;
    [SerializeField] Transform _poolActive;
    [SerializeField] Transform _poolInactive;
    [SerializeField] Transform[] _bulletSpawnPoses;
    
    public bool ShootIsEnable { get; set; }
    
    Pool<PooledObject> _bulletsPool;
    Pool<PooledObject> _shootFxPool;
    Pool<EventInstance> _shootingSoundsPool;
    CompositeDisposable _disposable = new();

#endregion

#region Publie methods

    public void Init(IdleNumber firePower, float fireRate)
    {
        CreatePools(firePower);

        Observable.Interval(TimeSpan.FromSeconds(fireRate))
                  .Where(_ => ShootIsEnable)
                  .Subscribe(_ => Shoot())
                  .AddTo(_disposable);
    }

    public void Deinit()
    {
        _disposable.Dispose();
        _bulletsPool.Clear();
        _shootFxPool.Clear();
        _shootingSoundsPool.Clear();
    }

#endregion

#region Private methods

    void CreatePools(IdleNumber firePower)
    {
        _bulletsPool = new FactoryGO.Builder(_bullet)
                       .SetParents(_poolActive, _poolInactive)
                       .SetPreloadCount(ConstGameplay.BULLETS_SPAWN_COUNT)
                       .SetMaxCount(ConstGameplay.BULLETS_SPAWN_COUNT + 5)
                       .SetItemInitConfig(firePower)
                       .Build();

        _shootFxPool = new FactoryGO.Builder(_shootFx)
                       .SetParents(_poolActive, _poolInactive)
                       .SetPreloadCount(ConstGameplay.BULLETS_SPAWN_COUNT)
                       .SetMaxCount(ConstGameplay.BULLETS_SPAWN_COUNT + 5)
                       .SetItemInitConfig(firePower)
                       .Build();

        _shootingSoundsPool = new FactoryFmodEvents.Builder(FmodEvents.Instance.Shoot)
                              .SetPreloadCount(3)
                              .SetMaxCount(5)
                              .Build();
    }

    void Shoot()
    {
        _shootingSoundsPool.Get().start();

        for (int i = 0; i < _bulletSpawnPoses.Length; i++)
        {
            var bullet = _bulletsPool.Get();
            bullet.transform.position = _bulletSpawnPoses[i].position;
            bullet.gameObject.SetActive(true);
            bullet.Set();

            var shootFx = _shootFxPool.Get();
            shootFx.transform.position = _bulletSpawnPoses[i].position;
            shootFx.Set();
        }
    }

#endregion
}
}