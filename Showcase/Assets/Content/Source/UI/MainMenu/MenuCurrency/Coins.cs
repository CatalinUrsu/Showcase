using R3;

namespace Source.UI
{
public class Coins : Currency
{
    void Awake() => _progressModel.CoinsRef.Subscribe(UpdateCurrencyText).AddTo(gameObject);
}
}