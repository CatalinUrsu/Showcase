using R3;
using System;

namespace Source.Data
{
public interface IShopService
{
    Observable<EShopAction> Actions { get; }

    bool TryBuyOrUpgradeShip(Guid id);
    bool TrySelectShip(Guid id);
    bool TryBuyOrUpgradeWeapon(Guid id);
    bool TrySelectWeapon(Guid id);
}
}
