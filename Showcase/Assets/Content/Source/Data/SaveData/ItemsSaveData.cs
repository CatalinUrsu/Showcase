using System;
using System.Collections.Generic;

namespace Source.Data
{
/// <summary>
/// Save file format of <see cref="ItemsModel"/>. Fields only.
/// </summary>
[Serializable]
public class ItemsSaveData
{
    public Dictionary<Guid, WeaponSaveData> WeaponsData = new();
    public Dictionary<Guid, ShipSaveData> ShipsData = new();
}
}
