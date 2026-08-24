using IdleNumbers;

namespace Source
{
public interface IProgressModelController
{
    IProgressModel IModel { get; }

    void SetLevel(int value);
    void AddLevel(int value = 1);

    void SetCoins(IdleNumber value);
    void AddCoins(IdleNumber value);
    void SpendCoins(IdleNumber value);

    void SetDiamonds(IdleNumber value);
    void AddDiamonds(IdleNumber value);
    void SpendDiamonds(IdleNumber value);

    void SetUsedShipIdx(int shipIdx);
    void SetUsedWeaponIdx(int weaponsIdx);

    void AscendProgress(int diamonds);
}
}