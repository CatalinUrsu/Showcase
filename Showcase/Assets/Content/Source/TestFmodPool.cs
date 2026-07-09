using UnityEngine;
using Source.Gameplay;
using Helpers.PoolSystem;

public class TestFmodPool : MonoBehaviour
{
    [SerializeField] Bullet _bulletPrefab;
    [SerializeField] Transform _poolActive;
    [SerializeField] Transform _poolInactive;

    Pool<PooledObject> _bulletPool;
    
    void TestPool()
    {
        var bulletPower = 10f;

        // Create pool for often-used sounds.
        _bulletPool = new Factory.Builder(_bulletPrefab)
                      .SetConfig(bulletPower)      // Pass shared init data to each created object.
                      .SetParents(_poolActive, _poolInactive) // Move active/inactive objects under different parents.
                      .SetPreloadCount(10)          // Create 10 objects on pool init.
                      .SetMaxCount(15)              // Keep up to 15 inactive objects.
                      .Build();
        
        // Get object from pool.
        PooledObject bullet = _bulletPool.Get();
        PooledObject bullet2 = _bulletPool.Get();
        
        // Apply post-spawn setup, if need to set different data each time (optional)
        bullet.Set();
        
        // Return specific object to pool 
        _bulletPool.Release(bullet);
        
        // Return all active objects to the pool.
        _bulletPool.ReleaseAll();
        
        // Destroy pooled objects and clear pool data.
        _bulletPool.Clear();
    }
}