using System;
using IdleNumbers;

namespace Source
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