using Helpers;
using DG.Tweening;
using UnityEngine;
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

     Sequence _animTween;

#endregion

#region Public methods

    public void Deinit() => _animTween.Kill();

    public async UniTask Show()
    {
        await _animTween.FinishAndGetNew()
                        .Append(PopupRT.DOMove(PositionShow.position, AnimDuration)
                                       .From(PositionInit.position)
                                       .SetEase(EaseShow))
                        .SetUpdate(true)
                        .AwaitForComplete();
    }

    public async UniTask Hide()
    {
        await _animTween.FinishAndGetNew()
                        .Append(PopupRT.DOMove(PositionHide.position, AnimDuration)
                                       .From(PositionShow.position)
                                       .SetEase(EaseHide))
                        .SetUpdate(true)
                        .AwaitForComplete();
    }

#endregion
}
}