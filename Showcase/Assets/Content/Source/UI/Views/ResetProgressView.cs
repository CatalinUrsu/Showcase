using R3;
using Helpers;
using DG.Tweening;
using Source.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using Zenject;

namespace Source.UI
{
[RequireComponent(typeof(MenuItemAnimation))]
public class ResetProgressView : MonoBehaviour, IResetProgressView
{
#region Fields

    [Space]
    [SerializeField] ButtonBase _itemButton;

    [SerializeField] Image _imgBg;
    [SerializeField] Color _colorActive;
    [SerializeField] Color _colorInactive;

    [Space]
    [SerializeField] RawImage _imgRawIcons;
    [SerializeField] Vector2 _activeMoveSpeed;
    [SerializeField] Vector2 _inactiveMoveSpeed;

    [Space]
    [SerializeField] LocalizeStringEvent _localizedStringEventBonus;
    [SerializeField] LocalizeStringEvent _localizedStringEventRequire;

    Rect _imgIconsUVRect;
    Vector2 _rawImgSpeed;
    Tween _speedChangeTween;
    ResetProgressPresenter _resetProgressPresenter;

    [Inject] ResetProgressPresenter.Factory _presenterFactory;

#endregion

#region Public methods

    void OnDestroy()
    {
        _speedChangeTween.CheckAndEnd();
        _resetProgressPresenter.Dispose();
    }

    public void Init()
    {
        _itemButton.Init();
        _itemButton.Btn.onClick.AddListener(OnSelect_handler);

        _resetProgressPresenter = _presenterFactory.Create(this);
        _rawImgSpeed = _inactiveMoveSpeed * Time.deltaTime;

        SetRawImageMovement();
    }

    public void OnChangeLvl_handler(bool reachedMinBonusLvl, int progressResetBonus)
    {
        _imgBg.color = reachedMinBonusLvl ? _colorActive : _colorInactive;

        SetRawImageSpeed(reachedMinBonusLvl);
        SetText(reachedMinBonusLvl, progressResetBonus);
    }

#endregion

#region Private methods

    void OnSelect_handler() => _resetProgressPresenter.TryResetProgress();

    void SetRawImageSpeed(bool reachedMinBonusLvl)
    {
        var newSpeed = (reachedMinBonusLvl ? _activeMoveSpeed : _inactiveMoveSpeed) * Time.deltaTime;

        if (newSpeed == _rawImgSpeed) return;

        _speedChangeTween.CheckAndEnd();
        _speedChangeTween = DOTween.To(() => _rawImgSpeed, x => _rawImgSpeed = x, newSpeed, 1);
    }

    void SetText(bool reachedMinBonusLvl, int progressResetBonus)
    {
        _localizedStringEventBonus.gameObject.SetActive(reachedMinBonusLvl);
        _localizedStringEventRequire.gameObject.SetActive(!reachedMinBonusLvl);

        if (reachedMinBonusLvl)
            (_localizedStringEventBonus.StringReference["0"] as StringVariable)!.Value = $"{progressResetBonus} {ConstSpriteAssets.SPRITE_TEXT_DIAMOND}";
        else
            (_localizedStringEventRequire.StringReference["0"] as StringVariable)!.Value = $"{ConstUpgradeItems.RESET_PROGRESS_MIN_LVL}";
    }

    void SetRawImageMovement()
    {
        _imgIconsUVRect = _imgRawIcons.uvRect;

        Observable.EveryUpdate()
                  .Where(_ => this != null && gameObject.activeInHierarchy)
                  .Subscribe(_ =>
                  {
                      _imgIconsUVRect.x += _rawImgSpeed.x;
                      _imgIconsUVRect.y += _rawImgSpeed.y;
                      _imgRawIcons.uvRect = _imgIconsUVRect;
                  })
                  .AddTo(gameObject);
    }

#endregion
}
}