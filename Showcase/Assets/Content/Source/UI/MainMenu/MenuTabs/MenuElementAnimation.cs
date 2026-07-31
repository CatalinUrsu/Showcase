using DG.Tweening;
using UnityEngine;

namespace Source.UI
{
public class MenuElementAnimation : MonoBehaviour
{
    [SerializeField] CanvasGroup _cgItem;

    public Tween GetShowAnim() => _cgItem.DOFade(1, ConstUIAnimation.UI_ANIM_DUR);

    public void ShowInstant() => _cgItem.alpha = 1;

    public Tween GetHideAnim() => _cgItem.DOFade(0, ConstUIAnimation.UI_ANIM_DUR);

    public void HideInstant() => _cgItem.alpha = 0;
}
}