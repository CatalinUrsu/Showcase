using System;
using System.Linq;
using System.Collections.Generic;

namespace Source.Data
{
/// <summary>
/// All shop items. Synced with SO list on creation: saved entries are kept, obsolete removed, new added from init data.
/// </summary>
public class ItemsModel
{
#region Fields

    readonly Dictionary<Guid, WeaponModel> _weapons;
    readonly Dictionary<Guid, ShipModel> _ships;
    readonly Dictionary<Guid, WeaponInitData> _initWeaponsData;
    readonly Guid[] _weaponsOrderedIds;
    readonly Guid[] _shipsOrderedIds;

#endregion

#region Public methods

    public ItemsModel(ItemsSaveData data,
                      Dictionary<Guid, WeaponInitData> initWeaponsData,
                      Dictionary<Guid, ShipInitData> initShipsData)
    {
        _initWeaponsData = initWeaponsData;
        _weaponsOrderedIds = initWeaponsData.Keys.ToArray();
        _shipsOrderedIds = initShipsData.Keys.ToArray();

        _weapons = initWeaponsData.ToDictionary(pair => pair.Key,
                                                pair => data.WeaponsData.TryGetValue(pair.Key, out var saved)
                                                            ? new WeaponModel(saved)
                                                            : new WeaponModel(pair.Value));

        _ships = initShipsData.ToDictionary(pair => pair.Key,
                                            pair => data.ShipsData.TryGetValue(pair.Key, out var saved)
                                                        ? new ShipModel(saved)
                                                        : new ShipModel(pair.Value));
    }

    public WeaponModel GetWeapon(Guid key) => _weapons[key];

    public WeaponModel GetWeapon(int idx) => _weapons[_weaponsOrderedIds[idx]];

    public int GetWeaponIdx(Guid key) => Array.IndexOf(_weaponsOrderedIds, key);

    public ShipModel GetShip(Guid key) => _ships[key];

    public ShipModel GetShip(int idx) => _ships[_shipsOrderedIds[idx]];

    public int GetShipIdx(Guid key) => Array.IndexOf(_shipsOrderedIds, key);

    // Ships stay after ascending by design, only weapons are reset
    public void ResetOnAscending()
    {
        foreach (var pair in _initWeaponsData)
            _weapons[pair.Key].Reset(pair.Value);
    }

    public ItemsSaveData ToSaveData()
    {
        return new ItemsSaveData
        {
            WeaponsData = _weapons.ToDictionary(pair => pair.Key, pair => pair.Value.ToSaveData()),
            ShipsData = _ships.ToDictionary(pair => pair.Key, pair => pair.Value.ToSaveData())
        };
    }

#endregion
}
}
