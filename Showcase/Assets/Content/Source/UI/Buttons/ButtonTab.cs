using Helpers;
using UnityEngine;
using DG.Tweening;
using Source.Audio;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class ButtonTab : ButtonBase
{
#region Fields

    [SerializeField] CanvasGroup _canvasGroup;

    public RectTransform _rtRef => _rt ??= GetComponent<RectTransform>();

#endregion

    public override void Init()
    {
        base.Init();

        onClick.AddListener(() => FmodEvents.Instance.BtnClick.PlayOneShot());
    }

    public async UniTask Select(bool skipAnimation, CancellationTokenSource cts)
    {
        float duration = ConstUIAnimation.GetAnimDuration(skipAnimation);
        await UniTask.WhenAll(_canvasGroup.DOFade(1, duration).ToUniTask(TweenCancelBehaviour.Complete, cts.Token),
                              _rtRef.DOAnchorPosY(-100, duration).ToUniTask(TweenCancelBehaviour.Complete, cts.Token));
    }

    public async UniTask Deselect(bool skipAnimation, CancellationTokenSource cts)
    {
        float duration = ConstUIAnimation.GetAnimDuration(skipAnimation);
        await UniTask.WhenAll(_canvasGroup.DOFade(.5f, duration).ToUniTask(TweenCancelBehaviour.Complete, cts.Token),
                              _rtRef.DOAnchorPosY(0, duration).ToUniTask(TweenCancelBehaviour.Complete, cts.Token));
    }
    
}
}