using Helpers;
using Helpers.UI;
using DG.Tweening;
using UnityEngine;

namespace Source.UI
{
public class ButtonBase : ButtonHelper
{
    Tween _clickTween;
    
    public override void Init()
    {
        base.Init();

        OnPointerDown += OnPointerDown_handler;
        OnPointerUp += OnPointerUp_handler;
    }
    
    void OnPointerDown_handler() => PlayPointerAnim(-ConstUIAnimation.ITEM_ANIM_SIZE);

    void OnPointerUp_handler() => PlayPointerAnim(ConstUIAnimation.ITEM_ANIM_SIZE);

    void PlayPointerAnim(Vector2 sizeDelta)
    {
        _clickTween?.CheckAndEnd();
        _clickTween = RT.DOSizeDelta(sizeDelta, ConstUIAnimation.UI_ANIM_DUR)
                        .SetRelative()
                        .SetUpdate(true);
    }
}
}