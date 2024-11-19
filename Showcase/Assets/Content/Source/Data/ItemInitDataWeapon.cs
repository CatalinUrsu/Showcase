using System;
using IdleNumbers;
using UnityEngine;

namespace Source.Datas
{
[Serializable]
public class ItemInitDataWeapon : ItemInitData
{
    [Space]
    public IdleNumber FirePower;
    public float FireRate;
}
}