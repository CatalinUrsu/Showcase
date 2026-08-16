using Helpers;
using Zenject;
using Helpers.UI;
using UnityEngine;
using DG.Tweening;
using Source.Data;
using Helpers.Audio;

namespace Source.UI
{
public class ButtonBase : ButtonHelper
{
    Tween _clickTween;
    [Inject] FmodEventsSo _fmodEvents;

    public override void Init()
    {
        base.Init();

        Btn.onClick.AddListener(PlayClickSound);
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

    void PlayClickSound() => _fmodEvents.BtnClick.PlayOneShot();
}
}