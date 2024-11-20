using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Source.UI
{
public class MenuElementAnimation : MonoBehaviour
{
    [SerializeField] CanvasGroup _cgItem;
    
    public Tween GetShowAnim(bool skipAnimation) =>
        _cgItem.DOFade(1, ConstUIAnimation.GetAnimDuration(skipAnimation));

    public UniTask GetHideAnim(bool skipAnimation, CancellationToken token) =>
        _cgItem.DOFade(0, ConstUIAnimation.GetAnimDuration(skipAnimation)).ToUniTask(TweenCancelBehaviour.Complete, token);
}
}