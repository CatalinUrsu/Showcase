using TMPro;
using Zenject;
using IdleNumbers;
using UnityEngine;
using Source.Data;

namespace Source.UI
{
public class Currency : MonoBehaviour
{
    [SerializeField] string _txtSpriteAsset;
    [SerializeField] protected TextMeshProUGUI _txtCurrency;
    
    protected ProgressModel _progressModel;

    [Inject]
    public void Construct(ProgressModel progress) => _progressModel = progress;

    protected void UpdateCurrencyText(IdleNumber currency) => _txtCurrency.SetText(currency.AsString() + _txtSpriteAsset);
}
}