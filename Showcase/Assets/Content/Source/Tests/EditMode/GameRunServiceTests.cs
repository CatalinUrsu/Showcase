using System;
using Source.Data;
using UnityEngine;
using NUnit.Framework;

namespace Source.Tests
{
public class GameRunServiceTests
{
    GameBalanceSO _balance;
    GameRunModel _gameRun;

    [SetUp]
    public void SetUp()
    {
        _balance = ScriptableObject.CreateInstance<GameBalanceSO>();
        _balance.CoinsLvlBonusMultiplier = .25f;
        _balance.ProgressTarget = 10;
        _gameRun = new GameRunModel();
        _gameRun.StartRun();
    }

    [TearDown]
    public void TearDown() => UnityEngine.Object.DestroyImmediate(_balance);

    [TestCase(1, 0, 125)]
    [TestCase(2, 0, 150)]
    [TestCase(1, 100, 250)]
    public void AddCoinsReward_LevelAndShipBonus_AddsCoinsWithBonuses(int lvl, float shipBonusPercent, double expectedCoins)
    {
        var progress = TestData.CreateProgress(lvl);
        var service = CreateService(progress, shipBonusPercent);

        service.AddCoinsReward(rewardPoints: 1, coins: 100);

        Assert.AreEqual(expectedCoins, progress.Coins.CurrentValue.Value, 1e-6);
        Assert.AreEqual(expectedCoins, _gameRun.CollectedCoins.CurrentValue.Value, 1e-6);
    }

    [Test]
    public void AddCoinsReward_PlayerKilled_AddsNothing()
    {
        var progress = TestData.CreateProgress();
        var service = CreateService(progress);
        _gameRun.KillPlayer();

        service.AddCoinsReward(rewardPoints: 1, coins: 100);

        Assert.AreEqual(0, progress.Coins.CurrentValue.Value);
        Assert.AreEqual(0, _gameRun.Progress.CurrentValue);
    }

    [Test]
    public void AddCoinsReward_ProgressReachesTarget_AddsLevelAndResetsRunProgress()
    {
        var progress = TestData.CreateProgress(lvl: 1);
        var service = CreateService(progress);

        service.AddCoinsReward(rewardPoints: _balance.ProgressTarget, coins: 1);

        Assert.AreEqual(2, progress.Lvl.CurrentValue);
        Assert.AreEqual(0, _gameRun.Progress.CurrentValue);
    }

    GameRunService CreateService(ProgressModel progress, float shipBonusPercent = 0)
    {
        var items = TestData.CreateItems((Guid.NewGuid(), TestData.Ship(enemyCoinBonus: shipBonusPercent, isBought: true, isSelected: true)));
        return new GameRunService(_gameRun, progress, items, _balance);
    }
}
}
