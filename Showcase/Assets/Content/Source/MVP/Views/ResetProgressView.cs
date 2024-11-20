using UniRx;
using Helpers;
using Source.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Source.MVP
{
[RequireComponent(typeof(MenuElementAnimation))]
public class ResetProgressView : MonoBehaviour, IViewResetProgress
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

#endregion

#region Public methods

    void OnDestroy() => _speedChangeTween.CheckAndEnd();

    public void Init()
    {
        _itemButton.Init();
        _itemButton.onClick.AddListener(OnSelect_handler);
        
        _resetProgressPresenter = new ResetProgressPresenter(gameObject,this);
        _rawImgSpeed = _inactiveMoveSpeed;
        
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

    void OnSelect_handler() => _resetProgressPresenter.OnClick_handler();

    void SetRawImageSpeed(bool reachedMinBonusLvl)
    {
        var newSpeed = reachedMinBonusLvl ? _activeMoveSpeed : _inactiveMoveSpeed;
        
         if(newSpeed == _rawImgSpeed) return;

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
                  .Where(_ => this!=null && gameObject.activeInHierarchy)
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