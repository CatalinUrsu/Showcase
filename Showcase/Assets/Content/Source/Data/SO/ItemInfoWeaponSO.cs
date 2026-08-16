using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/Items/WeaponInfo", fileName = "WeaponInfo_", order = 1)]
public class ItemInfoWeaponSO : ScriptableObject
{
    public ItemIdSo IdSo;
    public WeaponInitData InitData;
}
}