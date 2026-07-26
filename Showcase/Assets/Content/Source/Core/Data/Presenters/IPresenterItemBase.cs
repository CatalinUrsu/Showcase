using System;

namespace Source
{
public interface IPresenterItemBase : IDisposable
{
    void BuyOrUpgradeItem();
    void SelectItem();
}
}