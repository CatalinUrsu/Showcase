using IdleNumbers;

namespace Source.MVP
{
public interface IViewItem
{
    void OnUpdateSolvency_handler(bool isEnough);
    void OnChangeBoughtState_handler(bool isBought, IdleNumber price);
    void OnChangeSelectState_handler(bool isBought, bool isSelect);
}
}