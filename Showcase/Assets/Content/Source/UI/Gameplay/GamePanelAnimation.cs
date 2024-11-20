using Helpers;
using DG.Tweening;
using UnityEngine;
using Source.Audio;
using Cysharp.Threading.Tasks;

namespace Source.UI.Gameplay
{
public class GamePanelAnimation : MonoBehaviour
{
#region Fields

    [SerializeField] Ease EaseShow = Ease.Linear;
    [SerializeField] Ease EaseHide = Ease.Linear;
    [SerializeField] float AnimDuration = .25f;

    [Space] 
    [SerializeField] RectTransform PopupRT;
    [SerializeField] RectTransform PositionInit;
    [SerializeField] RectTransform PositionShow;
    [SerializeField] RectTransform PositionHide;

    public Sequence ShowSequence;
    public Sequence HideSequence;

#endregion

#region Public methods

    public void Deinit()
    {
        ShowSequence.Kill();
        HideSequence.Kill();
    }

    public async UniTask Show()
    {
        ShowSequence.CheckAndEnd();
        ShowSequence = DOTween.Sequence().Pause().SetUpdate(true);
        ShowSequence.Append(PopupRT.DOMove(PositionShow.position, AnimDuration)
                                   .From(PositionInit.position)
                                   .SetEase(EaseShow));
            
        await ShowSequence.Play().AwaitForComplete();
    }

    public async UniTask Hide()
    {
        HideSequence.CheckAndEnd();
        HideSequence = DOTween.Sequence().Pause().SetUpdate(true);
        HideSequence.Append(PopupRT.DOMove(PositionHide.position, AnimDuration)
                                   .From(PositionShow.position)
                                   .SetEase(EaseHide));
        
        await HideSequence.Play().AwaitForComplete();
    }

#endregion
}
}