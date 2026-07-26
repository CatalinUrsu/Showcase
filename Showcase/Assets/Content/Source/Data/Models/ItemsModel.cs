using System;
using System.Collections.Generic;

namespace Source.Data
{
[Serializable]
public class ItemsModel
{
    public Dictionary<Guid, WeaponModel> WeaponsData { get; set; } = new();
    public Dictionary<Guid, ShipModel> ShipsData { get; set; } = new();
}
}