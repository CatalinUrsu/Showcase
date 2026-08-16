using Helpers.UI;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
[RequireComponent(typeof(ButtonBase))]
public class TabButtonMenu : TabButton
{
    [SerializeField] CanvasGroup _canvasGroup;

    void OnValidate()
    {
        _canvasGroup ??= GetComponent<CanvasGroup>();
        _btnHelper ??= GetComponent<ButtonBase>();
    }

    public override async UniTask Select(bool skipAnimation, CancellationToken token)
    {
        var duration = ConstUIAnimation.GetAnimDuration(skipAnimation);
        await UniTask.WhenAll(_canvasGroup.DOFade(1, duration).ToUniTask(TweenCancelBehaviour.Complete, token),
                              BtnHelper.RT.DOAnchorPosY(-100, duration).ToUniTask(TweenCancelBehaviour.Complete, token));
    }

    public override async UniTask Deselect(bool skipAnimation, CancellationToken token)
    {
        var duration = ConstUIAnimation.GetAnimDuration(skipAnimation);
        await UniTask.WhenAll(_canvasGroup.DOFade(.5f, duration).ToUniTask(TweenCancelBehaviour.Complete, token),
                              BtnHelper.RT.DOAnchorPosY(0, duration).ToUniTask(TweenCancelBehaviour.Complete, token));
    }
}
}