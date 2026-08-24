using System;

namespace Source
{
public interface IItemsModelController
{
    IWeaponModel GetWeaponModel(Guid key);
    IWeaponModel GetWeaponModel(int idx);
    int GetWeaponIdx(Guid key);
    void BuyWeapon(Guid key);
    void SelectWeapon(Guid key);
    void DeselectWeapon(Guid key);
    void UpdateWeapon(Guid key);

    IShipModel GetShipModel(Guid key);
    IShipModel GetShipModel(int idx);
    int GetShipIdx(Guid key);
    void BuyShip(Guid key);
    void SelectShip(Guid key);
    void DeselectShip(Guid key);
    void UpdateShip(Guid key);

    void ResetOnAscending();
}
}