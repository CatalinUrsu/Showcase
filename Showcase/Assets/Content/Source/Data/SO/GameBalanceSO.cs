using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(fileName = "GameBalance", menuName = "SO/Game Balance", order = 0)]
public class GameBalanceSO : ScriptableObject
{
    [Header("Run")]
    public int ProgressTarget = 10;
    public float CoinsLvlBonusMultiplier = .25f;

    [Header("Player")]
    public int BulletsSpawnCount = 20;

    [Header("Enemies")]
    public int EnemiesSpawnCount = 10;
    public float EnemiesSpawnFrequency = .75f;
    public float EnemyHpMultiplier = 1.3f;

    [Header("Weapon upgrades")]
    public float WeaponPriceMultiplier = 1.2f;
    public float WeaponPowerUpgrade = 1;
    public float WeaponFireRateUpgrade = -.005f;
    public float WeaponFireRateMin = .2f;

    [Header("Ship upgrades")]
    public float ShipPriceMultiplier = 1.2f;
    public float ShipBonusIncrease = .25f;

    [Header("Reset progress")]
    public float ResetProgressMultiplier = 2.36f;
    public int ResetProgressMinLvl = 5;
}
}
