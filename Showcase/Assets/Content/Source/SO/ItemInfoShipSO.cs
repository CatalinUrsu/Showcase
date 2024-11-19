using UnityEngine;
using Source.Datas;

namespace Source.SO.Items
{
[CreateAssetMenu(menuName = "SO/ShipInfo", fileName = "ShipInfo_")]
public class ItemInfoShipSO : ItemInfo
{
    public ItemInitDataShip ShipInfo;
}
}