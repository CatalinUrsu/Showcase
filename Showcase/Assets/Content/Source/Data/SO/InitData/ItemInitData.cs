using System;
using IdleNumbers;

namespace Source.Data
{
[Serializable]
public class ItemInitData
{
    public bool IsBought;
    public bool IsSelected;
    public IdleNumber BuyPrice;
    public IdleNumber UpgradePrice;

}
}