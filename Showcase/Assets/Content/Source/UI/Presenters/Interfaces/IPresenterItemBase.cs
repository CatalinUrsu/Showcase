using System;

namespace Source.UI
{
public interface IPresenterItemBase : IDisposable
{
    void BuyOrUpgradeItem();
    void SelectItem();
}
}