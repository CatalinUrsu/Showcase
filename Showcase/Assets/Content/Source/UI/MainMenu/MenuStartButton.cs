using TMPro;
using Helpers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Source.UI
{
public class MenuStartButton : MonoBehaviour
{
    [SerializeField] Button _startBtn;
    [SerializeField] TextMeshProUGUI _startBtnText;

    Sequence _startbuttonSequence;

    public void Init(IUIMenuFacade uiMenuFacade)
    {
        _startBtn.onClick.AddListener(OnClickStart_handler);
        PlayTextLoopAnim();
        return;
        
        void OnClickStart_handler()
        {
            uiMenuFacade.ClickStartGame_raise();
            PlayTextHideAnimation();
        }
    }
    
    public void Deinit() => _startbuttonSequence.CheckAndEnd(false);
    

    void PlayTextLoopAnim()
    {
        _startbuttonSequence = DOTween.Sequence()
                                      .Append(_startBtnText.rectTransform.DOScale(1.25f, .5f).From(1f))
                                      .Join(_startBtnText.DOFade(.1f, .5f).From(.5f))
                                      .SetLoops(-1, LoopType.Yoyo);
    }

    void PlayTextHideAnimation()
    {
        _startbuttonSequence.CheckAndEnd(false);
        _startbuttonSequence = DOTween.Sequence()
                                      .Append(_startBtnText.rectTransform.DOMoveX(Screen.width, .5f)
                                                           .SetRelative()
                                                           .SetEase(Ease.InBack))
                                      .Join(_startBtnText.DOFade(.5f, 0));
    }
}
}