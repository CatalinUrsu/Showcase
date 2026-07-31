using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class ButtonTab : ButtonBase
{
    [SerializeField] CanvasGroup _canvasGroup;

    public async UniTask Select(bool skipAnimation, CancellationToken token)
    {
        var duration = ConstUIAnimation.GetAnimDuration(skipAnimation);
        await UniTask.WhenAll(_canvasGroup.DOFade(1, duration).ToUniTask(TweenCancelBehaviour.Complete, token),
                              RT.DOAnchorPosY(-100, duration).ToUniTask(TweenCancelBehaviour.Complete, token));
    }

    public async UniTask Deselect(bool skipAnimation, CancellationToken token)
    {
        var duration = ConstUIAnimation.GetAnimDuration(skipAnimation);
        await UniTask.WhenAll(_canvasGroup.DOFade(.5f, duration).ToUniTask(TweenCancelBehaviour.Complete, token),
                              RT.DOAnchorPosY(0, duration).ToUniTask(TweenCancelBehaviour.Complete, token));
    }
}
}