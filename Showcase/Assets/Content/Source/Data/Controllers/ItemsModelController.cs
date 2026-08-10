using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Source.Data
{
public class ItemsModelController: IItemsModelController
{
#region Fields

    public readonly ItemsModel Model;
    readonly Dictionary<Guid, WeaponInitData> _initWeaponsData;
    readonly Dictionary<Guid, ShipInitData> _initShipsData;
    readonly Guid[] _weaponsOrderedIds;
    readonly Guid[] _shipsOrderedIds;

    public ItemsModelController(ItemsModel model,
                                Dictionary<Guid, WeaponInitData> initWeaponsData,
                                Dictionary<Guid, ShipInitData> initShipsData)
    {
        Model = model;
        _initWeaponsData = initWeaponsData;
        _initShipsData = initShipsData;
        _weaponsOrderedIds = initWeaponsData.Keys.ToArray();
        _shipsOrderedIds = initShipsData.Keys.ToArray();

        SetShipsInitData();
        SetWeaponsInitData();
    }

#endregion

#region Public methods

    //----------Weapons methods----------
    public IWeaponModel GetWeaponModel(Guid key) => Model.WeaponsData[key];
    
    public IWeaponModel GetWeaponModel(int idx) => Model.WeaponsData[_weaponsOrderedIds[idx]];

    public int GetWeaponIdx(Guid key) => Array.IndexOf(_weaponsOrderedIds, key);

    public void BuyWeapon(Guid key) => BuyItem(Model.WeaponsData[key]);

    public void SelectWeapon(Guid key) => SelectItem(Model.WeaponsData[key]);

    public void DeselectWeapon(Guid key) => DeselectItem(Model.WeaponsData[key]);

    public void UpdateWeapon(Guid key) => UpdateWeapon(Model.WeaponsData[key]);


    //----------Ships methods----------
    public IShipModel GetShipModel(Guid key) => Model.ShipsData[key];
    
    public IShipModel GetShipModel(int idx) => Model.ShipsData[_shipsOrderedIds[idx]];

    public int GetShipIdx(Guid key) => Array.IndexOf(_shipsOrderedIds, key);

    public void BuyShip(Guid key) => BuyItem(Model.ShipsData[key]);

    public void SelectShip(Guid key) => SelectItem(Model.ShipsData[key]);

    public void DeselectShip(Guid key) => DeselectItem(Model.ShipsData[key]);

    public void UpdateShip(Guid key) => UpdateShip(Model.ShipsData[key]);

    public void ResetItems()
    {
        foreach (var pair in _initWeaponsData) 
            ResetWeapon(Model.WeaponsData[pair.Key], pair.Value);
        
        foreach (var pair in _initShipsData) 
            ResetShip(Model.ShipsData[pair.Key], pair.Value);
    }

#endregion

#region Private methods
    
    void SetShipsInitData()
    {
        // Remove obsolete entries.
        foreach (var key in Model.ShipsData.Keys.ToArray())
        {
            if (!_initShipsData.ContainsKey(key))
                Model.ShipsData.Remove(key);
        }

        // Add newly introduced entries.
        foreach (var pair in _initShipsData) 
            Model.ShipsData.TryAdd(pair.Key, new ShipModel(pair.Value));
    }
    
    void SetWeaponsInitData()
    {
        // Remove obsolete entries.
        foreach (var key in Model.WeaponsData.Keys.ToArray())
        {
            if (!_initWeaponsData.ContainsKey(key))
                Model.WeaponsData.Remove(key);
        }

        // Add newly introduced entries.
        foreach (var pair in _initWeaponsData) 
            Model.WeaponsData.TryAdd(pair.Key, new WeaponModel(pair.Value));
    }

    static void BuyItem(ItemModel model) => model.IsBought.Value = true;

    static void SelectItem(ItemModel model) => model.IsSelected.Value = true;

    static void DeselectItem(ItemModel model) => model.IsSelected.Value = true;

    static void UpdateWeapon(WeaponModel model)
    {
        model.UpgradePrice.Value *= ConstUpgradeItems.WEAPON_PRICE_MULTIPLIER;
        model.FirePower.Value += ConstUpgradeItems.WEAPON_POWER_UPGRADE;
        model.FireRate.Value = Mathf.Clamp(model.FireRate.Value + ConstUpgradeItems.WEAPON_FIRE_RATE_UPGRADE, ConstUpgradeItems.WEAPON_FIRE_RATE_MIN, 5);
    }

    static void UpdateShip(ShipModel model)
    {
        model.UpgradePrice.Value *= ConstUpgradeItems.SHIP_PRICE_MULTIPLIER;
        model.EnemyCoinBonus.Value += ConstUpgradeItems.SHIP_BONUS_INCREASE;
    }

    static void ResetWeapon(WeaponModel model, WeaponInitData initData)
    {
        ResetItem(model, initData);
        model.FirePower.Value = initData.FirePower;
        model.FireRate.Value = initData.FireRate;
    }

    static void ResetShip(ShipModel model, ShipInitData initData)
    {
        ResetItem(model, initData);
        model.EnemyCoinBonus.Value = initData.EnemyCoinBonus;
    }

    static void ResetItem(ItemModel model, ItemInitData initData)
    {
        model.IsBought.Value = initData.IsBought;
        model.IsSelected.Value = initData.IsSelected;
        model.BuyPrice.Value = initData.BuyPrice;
        model.UpgradePrice.Value = initData.UpgradePrice;
    }

#endregion
}
}