using TMPro;
using System;
using Zenject;
using IdleNumbers;
using Source.Data;
using UnityEngine;
using Helpers.Audio;
using UnityEngine.UI;
using Coffee.UIExtensions;

namespace Source.UI
{
[RequireComponent(typeof(MenuElementAnimation))]
public class ItemView : MonoBehaviour, IItemView
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

    bool _solvency;
    protected Guid _id;
    protected IPresenterItemBase _presenterItem;
    protected IItemsModelController _itemsModelController;
    protected IProgressModelController _progressModelController;
    protected ISessionService _sessionService;
    protected FmodEventsSo _fmodEvents;

#endregion

#region Public methods

    [Inject]
    public void Construct(ISessionService sessionService,
                          IItemsModelController itemsModelController,
                          IProgressModelController progressModelController,
                          FmodEventsSo fmodEvents)
    {
        _sessionService = sessionService;
        _itemsModelController = itemsModelController;
        _progressModelController = progressModelController;
        _fmodEvents = fmodEvents;
    }

    public virtual void Init(ItemLookSO itemAppearance)
    {
        _id = itemAppearance.IdSo.GUID;
        _itemButton.Init(_fmodEvents.BtnClick);
        _itemButton.AddListener(OnSelect_handler);

        _buyBtn.onClick.AddListener(OnBuyClick_handler);
        _imgItem.sprite = itemAppearance.ItemSprite;

        SetItemInfo();
    }

    public void UpdateShineEffect(float effectLocation)
    {
        if (!_solvency) return;
        _shinyEffect.location = effectLocation;
    }

    public void UpdateSolvency(bool isEnough)
    {
        _solvency = isEnough;
        _buyBtn.interactable = _solvency;
        _buyBtn.targetGraphic.color = _solvency ? _buyBtnColorOn : _buyBtnColorOff;
        _txtPrice.alpha = _solvency ? 1 : .5f;

        if (_solvency) return;
        _shinyEffect.location = 0;
    }

    public virtual void UpdateBoughtState(bool isBought, IdleNumber price)
    {
        _fmodEvents.BtnBuy.PlayOneShot();
        _cgContent.alpha = isBought ? ConstUIAnimation.ITEM_AVAILABLE_ALPHA : ConstUIAnimation.ITEM_NOT_AVAILABLE_ALPHA;
        _borderBought.SetActive(isBought);
    }

    public void UpdateSelectState(bool isBought, bool isSelect)
    {
        if (isSelect)
            _fmodEvents.SelectItem.PlayOneShot();

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