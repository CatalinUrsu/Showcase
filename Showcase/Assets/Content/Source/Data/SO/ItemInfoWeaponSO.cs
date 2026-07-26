using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/WeaponInfo", fileName = "WeaponInfo_")]
public class ItemInfoWeaponSO : ScriptableObject
{
    public ItemIdSo IdSo;
    public WeaponInitData InitData;
}
}