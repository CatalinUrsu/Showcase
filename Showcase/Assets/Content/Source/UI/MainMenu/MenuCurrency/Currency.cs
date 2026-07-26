using TMPro;
using IdleNumbers;
using UnityEngine;

namespace Source.UI
{
public class Currency : MonoBehaviour
{
    [SerializeField] string _txtSpriteAsset;
    [SerializeField] protected TextMeshProUGUI _txtCurrency;

    protected void UpdateCurrencyText(IdleNumber currency) => _txtCurrency.SetText(currency.AsString() + _txtSpriteAsset);
}
}