using System;
using System.Linq;
using Source.Data;
using System.Collections.Generic;

namespace Source.Tests
{
/// <summary>
/// Builders for long Arrange parts: real models with given state.
/// </summary>
public static class TestData
{
    public static ProgressModel CreateProgress(int lvl = 1, int coins = 0, int diamonds = 0) =>
        new(new ProgressSaveData { Lvl = lvl, Coins = coins, Diamonds = diamonds });

    /// <summary>
    /// Items with given ships (order = shop order) and one bought weapon.
    /// </summary>
    public static ItemsModel CreateItems(params (Guid id, ShipInitData data)[] ships)
    {
        var shipsData = ships.ToDictionary(ship => ship.id, ship => ship.data);
        var weaponsData = new Dictionary<Guid, WeaponInitData> { [Guid.NewGuid()] = new() { IsBought = true, IsSelected = true } };

        return new ItemsModel(new ItemsSaveData(), weaponsData, shipsData);
    }

    public static ShipInitData Ship(int buyPrice = 0, float enemyCoinBonus = 0, bool isBought = false, bool isSelected = false) =>
        new() { BuyPrice = buyPrice, UpgradePrice = buyPrice, EnemyCoinBonus = enemyCoinBonus, IsBought = isBought, IsSelected = isSelected };
}
}
