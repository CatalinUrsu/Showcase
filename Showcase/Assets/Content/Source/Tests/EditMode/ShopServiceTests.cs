using System;
using NUnit.Framework;
using Source.Data;
using UnityEngine;

namespace Source.Tests
{
public class ShopServiceTests
{
    readonly Guid _firstShipId = Guid.NewGuid();
    readonly Guid _secondShipId = Guid.NewGuid();

    GameBalanceSO _balance;
    ItemsModel _items;

    [SetUp]
    public void SetUp()
    {
        _balance = ScriptableObject.CreateInstance<GameBalanceSO>();
        _items = TestData.CreateItems((_firstShipId, TestData.Ship(isBought: true, isSelected: true)),
                                      (_secondShipId, TestData.Ship(buyPrice: 10)));
    }

    [TearDown]
    public void TearDown() => UnityEngine.Object.DestroyImmediate(_balance);

    [Test]
    public void TryBuyOrUpgradeShip_NotEnoughDiamonds_ReturnsFalseAndShipStaysLocked()
    {
        var progress = TestData.CreateProgress(diamonds: 9);
        var shop = new ShopService(_items, progress, _balance);

        var result = shop.TryBuyOrUpgradeShip(_secondShipId);

        Assert.IsFalse(result);
        Assert.IsFalse(_items.GetShip(_secondShipId).IsBought.CurrentValue);
        Assert.AreEqual(9, progress.Diamonds.CurrentValue.Value);
    }

    [Test]
    public void TryBuyOrUpgradeShip_EnoughDiamonds_SelectsBoughtShipAndDeselectsPrevious()
    {
        var progress = TestData.CreateProgress(diamonds: 10);
        var shop = new ShopService(_items, progress, _balance);

        var result = shop.TryBuyOrUpgradeShip(_secondShipId);

        Assert.IsTrue(result);
        Assert.IsTrue(_items.GetShip(_secondShipId).IsSelected.CurrentValue);
        Assert.IsFalse(_items.GetShip(_firstShipId).IsSelected.CurrentValue);
        Assert.AreEqual(_secondShipId, progress.UsedShipId.CurrentValue);
        Assert.AreEqual(0, progress.Diamonds.CurrentValue.Value);
    }
}
}
