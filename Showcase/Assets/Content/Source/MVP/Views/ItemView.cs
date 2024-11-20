using TMPro;
using Source.UI;
using IdleNumbers;
using UnityEngine;
using UnityEngine.UI;
using Source.SO.Items;
using Coffee.UIExtensions;

namespace Source.MVP
{
[RequireComponent(typeof(MenuElementAnimation))]
public class ItemView : MonoBehaviour, IViewItem
{
#region Fields

    [Space]
    [SerializeField] protected ButtonBase _itemButton;
    [SerializeField] CanvasGroup _cgContent;
    [SerializeField] Button _buyBtn;
    [SerializeField] Color _buyBtnColorOn;
    [SerializeField] Color _buyBtnColorOff;
    [SerializeField] protected TextMeshProUGUI _txtPrice;

    [Space] 
    [SerializeField] Image _imgItem;
    [SerializeField] GameObject _borderBought;
    [SerializeField] GameObject _bordeSelected;

    [Space]
    [SerializeField] ShinyEffectForUGUI _shinyEffect;
    
    protected int _itemIdx;
    protected IPresenterItemBase _presenterItem;
    bool _solvency;

#endregion

#region Public methods

    public virtual void Init(ItemLookSO appearenceShip, int itemIdx)
    {
        _itemButton.Init();
        _itemButton.onClick.AddListener(OnSelect_handler);
        
        _buyBtn.onClick.AddListener(OnBuyClick_handler);
        _imgItem.sprite = appearenceShip.ItemSprite;
        _itemIdx = itemIdx;

        SetItemInfo();
    }
    
    public void UpdateShineEffect(float effectLocation)
    {
        if (!_solvency) return;
        _shinyEffect.location = effectLocation;
    }

    public void OnUpdateSolvency_handler(bool isEnough)
    {
        _solvency = isEnough;
        _buyBtn.interactable = _solvency;
        _buyBtn.targetGraphic.color = _solvency ? _buyBtnColorOn : _buyBtnColorOff;
        _txtPrice.alpha = _solvency ? 1 : .5f;

        if (_solvency) return;
        _shinyEffect.location = 0;
    }

    public virtual void OnChangeBoughtState_handler(bool isBought, IdleNumber price)
    {
        _cgContent.alpha = isBought ? ConstUIAnimation.ITEM_AVAILABLE_ALPHA : ConstUIAnimation.ITEM_NOT_AVAILABLE_ALPHA;
        _borderBought.SetActive(isBought);
    }

    public void OnChangeSelectState_handler(bool isBought, bool isSelect)
    {
        _borderBought.SetActive(isBought && !isSelect);
        _bordeSelected.SetActive(isSelect);
    }

#endregion

#region Private methods

    protected virtual void SetItemInfo() { }
    protected virtual void OnBuyClick_handler() { }
    protected virtual void OnSelect_handler() { }

#endregion
}
}