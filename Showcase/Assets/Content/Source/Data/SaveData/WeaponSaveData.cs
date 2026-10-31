using System;
using IdleNumbers;

namespace Source.Data
{
[Serializable]
public class WeaponSaveData : ItemSaveData
{
    public IdleNumber FirePower;
    public float FireRate;
}
}
