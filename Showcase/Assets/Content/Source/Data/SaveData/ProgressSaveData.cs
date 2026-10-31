using System;
using IdleNumbers;

namespace Source.Data
{
/// <summary>
/// Save file format of <see cref="ProgressModel"/>. Fields only.
/// </summary>
[Serializable]
public class ProgressSaveData
{
    public int Lvl = 1;
    public IdleNumber Coins;
    public IdleNumber Diamonds;
    public int UsedShipIdx;
    public int UsedWeaponIdx;
}
}
