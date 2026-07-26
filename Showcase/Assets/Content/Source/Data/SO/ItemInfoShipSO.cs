using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/ShipInfo", fileName = "ShipInfo_")]
public class ItemInfoShipSO : ScriptableObject
{
    public ItemIdSo IdSo;
    public ShipInitData InitData;
}
}