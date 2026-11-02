using NUnit.Framework;

namespace Source.Tests
{
public class ProgressModelTests
{
    [Test]
    public void TrySpendCoins_PriceAboveBalance_ReturnsFalseAndKeepsCoins()
    {
        var progress = TestData.CreateProgress(coins: 5);

        var result = progress.TrySpendCoins(10);

        Assert.IsFalse(result);
        Assert.AreEqual(5, progress.Coins.CurrentValue.Value);
    }

    [Test]
    public void TrySpendCoins_PriceEqualsBalance_SpendsAll()
    {
        var progress = TestData.CreateProgress(coins: 10);

        var result = progress.TrySpendCoins(10);

        Assert.IsTrue(result);
        Assert.AreEqual(0, progress.Coins.CurrentValue.Value);
    }
}
}
