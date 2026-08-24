using R3;
using System;
using Zenject;
using UnityEngine;
using Source.Data;
using System.Linq;
using Helpers.Services;
using Helpers.PoolSystem;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Source.Game.Gameplay
{
public class EnemiesSpawner : MonoBehaviour, IEnemiesSpawner
{
#region Exernal Types

    [Serializable]
    public struct EnemyByChance
    {
        public int Chance;
        public EnemySO EnemySO;

        public int MinChance { get; set; }
        public int MaxChance{ get; set; }
    }

#endregion
    
#region Fields

    [SerializeField] EnemyByChance[] _enemiesByChances;
    
    [Space]
    [SerializeField] Transform _enemiesSpawnPoint;
    [SerializeField] Transform _poolActive;
    [SerializeField] Transform _poolInactive;

    [Space]
    [SerializeField] PooledFX _hitFx;
    [SerializeField] PooledFX _deathFx;
    
    int _chanceSum;
    bool _spawnEnable;
    Vector2 _enemySpawnPosition;
    TimeSpan _spawnInterval;
    AsyncOperationHandle<GameObject> _enemyAssetOpHandle;
    Pool<PooledObject> _pool;
    ICameraService _cameraService;
    
    FmodEventsSo _fmodEventsSo;
    EnemiesInitConfig _enemiesInitConfig;
    readonly CompositeDisposable _disposable = new();

#endregion

#region Public methods

    [Inject]
    public void Construct(ICameraService cameraService, FmodEventsSo fmodEventsSo)
    {
        _cameraService = cameraService;
        _fmodEventsSo = fmodEventsSo;
    }

    public async UniTask Init()
    {
        _enemiesInitConfig = new EnemiesInitConfig(_hitFx, _deathFx, _poolActive, _poolInactive, _fmodEventsSo);
        await LoadEnemyAsset();

        SetSpawnChance();
        SetSpawnPosition();
        SetSpawnLogic();
    }

    public void Deinit()
    {
        Addressables.Release(_enemyAssetOpHandle);
        _pool.Clear();
        _enemiesInitConfig.Deinit();
        _disposable.Dispose();
    }

    public void ToggleSpawning(bool enabe) => _spawnEnable = enabe;

#endregion

#region Private methods

    /// <summary>
    /// Addressables are not required here. I used it just as an axample of its usage and loading and unloading some asset 
    /// </summary>
    async UniTask LoadEnemyAsset()
    {
        _enemyAssetOpHandle = Addressables.LoadAssetAsync<GameObject>(ConstGameplay.ENEMIES_LOCATION_KEY);
        _enemyAssetOpHandle.Completed += InitPool;
        await _enemyAssetOpHandle.ToUniTask();
    }
    
    void InitPool(AsyncOperationHandle<GameObject> opHandle)
    {
        var enemyFacade = _enemyAssetOpHandle.Result.GetComponent<EnemyFacade>();
        _pool = new Factory.Builder(enemyFacade)
                             .SetConfig(_enemiesInitConfig)
                             .SetParents(_poolActive, _poolInactive)
                             .SetPreloadCount(ConstGameplay.ENEMIES_SPAWN_COUNT)
                             .Build();
    }

    /// <summary>
    /// Configures the spawn chance ranges for each enemy:<br/>
    /// 1) Sort the array of enemies<br/>
    /// 2) Calculate cumulative chance ranges <br/>
    /// 3) Assigning minimum and maximum chance values for each enemy.
    /// </summary>
    void SetSpawnChance()
    {
        _enemiesByChances = _enemiesByChances.OrderBy(enemy => enemy.Chance).ToArray();
        _chanceSum = _enemiesByChances.Sum(enemy => enemy.Chance);

        for (int i = 0; i < _enemiesByChances.Length; i++)
        {
            var currentEnemy = _enemiesByChances[i];

            currentEnemy.MinChance = i == 0 ? 0 : _enemiesByChances[i - 1].MaxChance;
            currentEnemy.MaxChance = currentEnemy.Chance + currentEnemy.MinChance;
            
            _enemiesByChances[i] = currentEnemy;
        }
    }

    void SetSpawnPosition()
    {
        var mainCamera = _cameraService.GetMainCamera();
        var posX = mainCamera.aspect * mainCamera.orthographicSize + 1;
        _enemySpawnPosition = new Vector2(posX, _enemiesSpawnPoint.position.y);
    }

    void SetSpawnLogic()
    {
        _disposable.Clear();
        _spawnInterval = TimeSpan.FromSeconds(ConstGameplay.ENEMIES_SPAWN_FREQUENCY);

        Observable.Interval(_spawnInterval)
                  .Where(_ => _spawnEnable)
                  .Subscribe(_ => SpawnEnemy())
                  .AddTo(_disposable);
        return;

        void SpawnEnemy()
        {
            var chance = UnityEngine.Random.Range(0, _chanceSum);
            var pooledObject = _pool.Get();
            var enemySO = _enemiesByChances.First(enemyByChance => chance >= enemyByChance.MinChance && chance < enemyByChance.MaxChance).EnemySO;
            var spawnPos = new Vector2(UnityEngine.Random.Range(-_enemySpawnPosition.x, _enemySpawnPosition.x), _enemySpawnPosition.y);

            pooledObject.transform.position = spawnPos;
            pooledObject.Set(enemySO);
        }
    }

#endregion
}
}