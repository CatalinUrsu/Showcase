using TMPro;
using IdleNumbers;
using UnityEngine;
using Zenject;

namespace Source.UI
{
public class Currency : MonoBehaviour
{
    [SerializeField] string _txtSpriteAsset;
    [SerializeField] protected TextMeshProUGUI _txtCurrency;
    
    protected IProgressModel _progressModel;

    [Inject]
    public void Construct(IProgressModelController progressModelController) => _progressModel = progressModelController.IModel;

    protected void UpdateCurrencyText(IdleNumber currency) => _txtCurrency.SetText(currency.AsString() + _txtSpriteAsset);
}
}