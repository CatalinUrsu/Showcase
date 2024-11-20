using TMPro;
using System;
using Source.UI;
using UnityEngine;
using DG.Tweening;
using Source.Player;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Helpers;

namespace Source.StateMachine
{
public class MenuMediator : MonoBehaviour
{
#region Fields

    [SerializeField] PlayerFacadeMenu _playerFacadeMenu;
    [SerializeField] TabsGroup _tabsGroup;

    [Header("Start Button")] 
    [SerializeField] Button _startBtn;
    [SerializeField] TextMeshProUGUI _startBtnText;

    public event Action OnClickStartGame;

    Sequence _startbuttonSequence;

#endregion

#region Public methods

    public async UniTask Init(Action<float> onUpdateProgress)
    {
        _playerFacadeMenu.Init();
        SetStartButtonAnim();

        await _tabsGroup.InitContent(onUpdateProgress);
    }

    public void Deinit()
    {
        _playerFacadeMenu.Deinit();
        _startbuttonSequence.CheckAndEnd(false);
    }

    public async UniTask ShowPlayer() => await _playerFacadeMenu.ShowPayer();

#endregion

#region Private methods

    void SetStartButtonAnim()
    {
        _startBtn.onClick.AddListener(OnClickStart_handler);

        _startbuttonSequence = DOTween.Sequence()
                                      .Append(_startBtnText.rectTransform.DOScale(1.25f, .5f).From(1f))
                                      .Join(_startBtnText.DOFade(.1f, .5f).From(.5f))
                                      .SetLoops(-1, LoopType.Yoyo);
    }

    void OnClickStart_handler()
    {
        OnClickStartGame?.Invoke();

        _startbuttonSequence.CheckAndEnd(false);
        _startbuttonSequence = DOTween.Sequence()
                                      .Append(_startBtnText.rectTransform.DOMoveX(Screen.width, .5f)
                                                           .SetRelative()
                                                           .SetEase(Ease.InBack))
                                      .Join(_startBtnText.DOFade(.5f, 0));
    }

#endregion
}
}