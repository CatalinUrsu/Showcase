using UnityEngine;
using Source.Datas;
using Source.Session;
using Source.SO.Items;
using System.Collections.Generic;

namespace Source
{
public class InitValuesForSaves : MonoBehaviour
{
    [SerializeField] ItemInfoWeaponSO[] _weaponsSO;
    [SerializeField] ItemInfoShipSO[] _ShipsSO;
    
    public void LoadSavedItems()
    {
        Dictionary<string, ItemInitDataWeapon> initWeaponsInfo = new Dictionary<string, ItemInitDataWeapon>();
        Dictionary<string, ItemInitDataShip> initShipsInfo = new Dictionary<string, ItemInitDataShip>();

        foreach (var weaponSo in _weaponsSO)
            initWeaponsInfo.Add(weaponSo.GUID, weaponSo.WeaponInfo);

        foreach (var shipSO in _ShipsSO)
            initShipsInfo.Add(shipSO.GUID, shipSO.ShipInfo);

        SessionManager.Current.SetInitDataForItems(initWeaponsInfo, initShipsInfo);
    }
}
}