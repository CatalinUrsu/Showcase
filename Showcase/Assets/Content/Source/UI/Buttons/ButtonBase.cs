using Helpers;
using DG.Tweening;
using UnityEngine;
using Source.Audio;

namespace Source.UI
{
public class ButtonBase : ButtonHelper
{
#region Fields

    [SerializeField] bool _playButtonSound = true;

    RectTransform _rt;
    Tween _clickTween;

#endregion

    public virtual void Init()
    {
        _rt = GetComponent<RectTransform>();

        OnTouchDown += () => OnTouch_handler(true);
        OnTouchUp += () => OnTouch_handler(false);

        if (_playButtonSound)
            onClick.AddListener(() => FmodEvents.Instance.BtnClick.PlayOneShot());
    }

    void OnTouch_handler(bool isPressed)
    {
        _clickTween?.CheckAndEnd();
        _clickTween = _rt.DOSizeDelta(isPressed ? -ConstUIAnimation.ITEM_ANIM_SIZE : ConstUIAnimation.ITEM_ANIM_SIZE, ConstUIAnimation.UI_ANIM_DUR)
                         .SetRelative()
                         .SetUpdate(true);
    }
}
}