using UnityEngine;
using System.Collections.Generic;
using Source.Data;

namespace Source
{
public class InitValuesForSaves : MonoBehaviour
{
    [SerializeField] ItemInfoWeaponSO[] _weaponsSO;
    [SerializeField] ItemInfoShipSO[] _ShipsSO;
    
    public void LoadSavedItems()
    {
        Dictionary<string, WeaponInitData> initWeaponsInfo = new Dictionary<string, WeaponInitData>();
        Dictionary<string, ShipInitData> initShipsInfo = new Dictionary<string, ShipInitData>();

        foreach (var weaponSo in _weaponsSO)
            initWeaponsInfo.Add(weaponSo.IdSo._guid, weaponSo.InitData);

        foreach (var shipSO in _ShipsSO)
            initShipsInfo.Add(shipSO.IdSo._guid, shipSO.InitData);

        SessionService.Current.SetInitDataForItems(initWeaponsInfo, initShipsInfo);
    }
}
}