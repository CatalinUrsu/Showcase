using R3;

namespace Source.UI
{
public class Coins : Currency
{
    void Awake() => Session.SessionManager.Current.Progress.Coins.Subscribe(UpdateCurrencyText).AddTo(gameObject);
}
}