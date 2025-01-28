using Helpers;
using Zenject;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Helpers.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Source.UI
{
public class SplashScreen : MonoBehaviour, ISplashScreen<ISplashScreenInfo>
{
#region Fields

    [SerializeField] LocalizeStringEvent _localizedLoadingProgress;
    [SerializeField] Image _imgLoadingBar;

    IServiceSplashScreen _serviceSplashScreen;
    IServiceLoadingProgress _serviceLoadingProgress;
    float _panelHeight;
    RectTransform _rt;
    Tween _splashScreenTween;

#endregion

#region Monobeh

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _panelHeight = _rt.rect.height;
        
        _localizedLoadingProgress.StringReference.Arguments= new object[] { new StringVariable() };
    }

#endregion
    
#region Public methods

    [Inject]
    public void Construct(IServiceSplashScreen serviceSplashScreen, IServiceLoadingProgress serviceLoadingProgress)
    {
        _serviceLoadingProgress = serviceLoadingProgress;
        _serviceSplashScreen = serviceSplashScreen;
        _serviceSplashScreen.RegisterSplashScreen(this);

        _serviceLoadingProgress.OnUpdateProgress += OnUpdateProgress_handler;
        _serviceLoadingProgress.OnUpdateLoadingPrompt += OnUpdateLoadingPrompt_handler;
    }

    public async UniTask ShowPanel(ISplashScreenInfo config)
    {
        var duration = config.SkipAnimation ? 0 : ConstUIAnimation.SPLASH_SCREEN_ANIM_DUR;
        _splashScreenTween.CheckAndEnd(false);
        _splashScreenTween = _rt.DOAnchorPosY(0, duration).SetUpdate(true);;
        await _splashScreenTween;
    }

    public async UniTask HidePanel()
    {
        _splashScreenTween.CheckAndEnd(false);
        _splashScreenTween = _rt.DOAnchorPosY(_panelHeight, ConstUIAnimation.SPLASH_SCREEN_ANIM_DUR).SetUpdate(true);;
        await _splashScreenTween;
    }

#endregion

#region Private methods

    void OnUpdateProgress_handler(float progress) => _imgLoadingBar.fillAmount = Mathf.Clamp(progress, 0, 1);

    void OnUpdateLoadingPrompt_handler(string target) => (_localizedLoadingProgress.StringReference.Arguments[0] as StringVariable)!.Value = target;

#endregion
}
}