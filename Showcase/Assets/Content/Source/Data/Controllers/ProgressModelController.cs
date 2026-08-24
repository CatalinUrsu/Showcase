using IdleNumbers;

namespace Source.Data
{
public class ProgressModelController : IProgressModelController
{
#region Fields

    public IProgressModel IModel => Model;
    public readonly ProgressModel Model;

#endregion

#region Public methods

    public ProgressModelController(ProgressModel model) => Model = model;

    public void SetLevel(int value) => Model.Lvl.Value = value;

    public void AddLevel(int value = 1) => Model.Lvl.Value += value;

    public void SetCoins(IdleNumber value) => Model.Coins.Value = value;

    public void AddCoins(IdleNumber value) => Model.Coins.Value += value;

    public void SpendCoins(IdleNumber value) => Model.Coins.Value -= value;

    public void SetDiamonds(IdleNumber value) => Model.Diamonds.Value = value;

    public void AddDiamonds(IdleNumber value) => Model.Diamonds.Value += value;

    public void SpendDiamonds(IdleNumber value) => Model.Diamonds.Value -= value;

    public void SetUsedShipIdx(int shipIdx) => Model.UsedShipIdx.Value = shipIdx;

    public void SetUsedWeaponIdx(int weaponsIdx) => Model.UsedWeaponIdx.Value = weaponsIdx;

    public void AscendProgress(int diamonds)
    {
        Model.Lvl.Value = 1;
        Model.Coins.Value = 0;
        Model.UsedWeaponIdx.Value = 0;
        Model.Diamonds.Value += diamonds;
    }

#endregion
}
}