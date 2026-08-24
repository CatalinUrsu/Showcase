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
        _animTween = _animTween.FinishAndGetNew()
                               .Append(PopupRT.DOMove(PositionShow.position, AnimDuration)
                                              .From(PositionInit.position)
                                              .SetEase(EaseShow))
                               .SetUpdate(true);
        
        await _animTween.AwaitForComplete();
    }

    public async UniTask Hide()
    {
        _animTween = _animTween.FinishAndGetNew()
                        .Append(PopupRT.DOMove(PositionHide.position, AnimDuration)
                                       .From(PositionShow.position)
                                       .SetEase(EaseHide))
                        .SetUpdate(true);
        
        await _animTween.AwaitForComplete();
    }

#endregion
}
}