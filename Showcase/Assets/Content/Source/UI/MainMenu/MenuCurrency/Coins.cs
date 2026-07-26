using R3;

namespace Source.UI
{
public class Coins : Currency
{
    void Awake() => Session.SessionService.Current.Progress.Coins.Subscribe(UpdateCurrencyText).AddTo(gameObject);
}
}