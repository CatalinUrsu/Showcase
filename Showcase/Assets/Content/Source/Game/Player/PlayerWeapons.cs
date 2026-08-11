using R3;
using System;
using Zenject;
using FMODUnity;
using UnityEngine;
using Source.Data;
using FMOD.Studio;
using IdleNumbers;
using Helpers.Audio;
using Helpers.PoolSystem;
using Source.Game.Gameplay;

namespace Source.Game.Player
{
public class PlayerWeapons : MonoBehaviour
{
#region Fields

    [SerializeField] Bullet _bullet;
    [SerializeField] PooledFX _shootFx;
    [SerializeField] Transform _poolActive;
    [SerializeField] Transform _poolInactive;
    [SerializeField] Transform[] _bulletSpawnPoses;

    bool _shootIsEnable;
    IdleNumber _firePower;
    EventReference _shootSfxRef;
    float _fireRate;
    
    Pool<PooledObject> _bulletsPool;
    Pool<PooledObject> _shootFxPool;
    Pool<EventInstance> _shootingSoundsPool;
    CompositeDisposable _disposable = new();

#endregion

#region Publie methods
    
    [Inject]
    public void Construct(IProgressModelController progressModelController,
                          IItemsModelController itemsModelController,
                          IGameRunModelController runModelController,
                          FmodEventsSo fmodEventsSo)
    {
        var usedWeaponIdx = progressModelController.IModel.UsedWeaponIdxRef.CurrentValue;
        var usedWeapon = itemsModelController.GetWeaponModel(usedWeaponIdx);
        _firePower = usedWeapon.FirePowerRef.CurrentValue;
        _fireRate = usedWeapon.FireRateRef.CurrentValue;
        _shootSfxRef = fmodEventsSo.Shoot;
    }

    public void Init()
    {
        CreatePools();

        Observable.Interval(TimeSpan.FromSeconds(_fireRate))
                  .Where(_ => _shootIsEnable)
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

    public void ToggleShooting(bool enable) => _shootIsEnable = enable;
    
#endregion

#region Private methods

    void CreatePools()
    {
        _bulletsPool = new Factory.Builder(_bullet)
                          .SetConfig(_firePower)
                          .SetParents(_poolActive, _poolInactive)
                          .SetPreloadCount(ConstGameplay.BULLETS_SPAWN_COUNT)
                          .SetMaxCount(ConstGameplay.BULLETS_SPAWN_COUNT + 5)
                          .Build();

        _shootFxPool = new Factory.Builder(_shootFx)
                          .SetParents(_poolActive, _poolInactive)
                          .SetPreloadCount(ConstGameplay.BULLETS_SPAWN_COUNT)
                          .SetMaxCount(ConstGameplay.BULLETS_SPAWN_COUNT + 5)
                          .Build();

        _shootingSoundsPool = new FactoryFmodEvents.Builder(_shootSfxRef)
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