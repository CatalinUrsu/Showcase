using IdleNumbers;

namespace Source
{
public interface IItemView
{
    void UpdateSolvency(bool isEnough);
    void UpdateBoughtState(bool isBought, IdleNumber price);
    void UpdateSelectState(bool isBought, bool isSelect);
}
}