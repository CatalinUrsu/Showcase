using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/Items/ShipInfo", fileName = "ShipInfo_", order = 0)]
public class ItemInfoShipSO : ScriptableObject
{
    public ItemIdSo IdSo;
    public ShipInitData InitData;
}
}