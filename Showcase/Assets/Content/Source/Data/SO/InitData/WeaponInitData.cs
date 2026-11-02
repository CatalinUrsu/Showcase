using System;
using IdleNumbers;
using UnityEngine;

namespace Source.Data
{
[Serializable]
public class WeaponInitData : ItemInitData
{
    [Space]
    public IdleNumber FirePower;
    public float FireRate;
}
}