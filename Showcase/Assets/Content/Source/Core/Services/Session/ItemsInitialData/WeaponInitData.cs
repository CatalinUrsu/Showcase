using System;
using IdleNumbers;
using UnityEngine;

namespace Source
{
[Serializable]
public class WeaponInitData : ItemInitData
{
    [Space]
    public IdleNumber FirePower;
    public float FireRate;
}
}