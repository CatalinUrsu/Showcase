using R3;

namespace Source.UI
{
public class Coins : Currency
{
    void Awake() => _progressModel.Coins.Subscribe(UpdateCurrencyText).AddTo(gameObject);
}
}