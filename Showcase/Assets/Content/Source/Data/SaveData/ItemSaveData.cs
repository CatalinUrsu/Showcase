using System;
using IdleNumbers;

namespace Source.Data
{
/// <summary>
/// Save file format of <see cref="ItemModel"/>. Fields only.
/// </summary>
[Serializable]
public class ItemSaveData
{
    public bool IsBought;
    public bool IsSelected;
    public IdleNumber BuyPrice;
    public IdleNumber UpgradePrice;
}
}
