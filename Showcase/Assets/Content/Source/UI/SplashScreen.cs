using Helpers;
using Zenject;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Helpers.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Source.UI
{
public class SplashScreen : MonoBehaviour, ISplashScreen
{
#region Fields

    [SerializeField] LocalizeStringEvent _localizedLoadingProgress;
    [SerializeField] Image _imgLoadingBar;

    ISplashScreenService _splashScreenService;
    IProgressTrackingService progressTrackingService;
    float _panelHeight;
    RectTransform _rt;
    Tween _splashScreenTween;

#endregion

#region Monobeh

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _panelHeight = _rt.rect.height;
    }

#endregion

#region Public methods

    [Inject]
    public void Construct(ISplashScreenService splashScreenService, IProgressTrackingService progressTrackingService)
    {
        this.progressTrackingService = progressTrackingService;
        _splashScreenService = splashScreenService;
        _splashScreenService.RegisterSplashScreen(ConstSceneNames.LOADING_SCENE, this);

        this.progressTrackingService.OnUpdateProgress += OnUpdateProgress_handler;
        this.progressTrackingService.OnUpdateLoadingTip += OnUpdateLoadingTip_handler;
    }

    public async UniTask ShowPanel(bool skipAnimation)
    {
        var duration = skipAnimation ? 0 : ConstUIAnimation.SPLASH_SCREEN_ANIM_DUR;
        _splashScreenTween.CheckAndEnd(false);
        _splashScreenTween = _rt.DOAnchorPosY(0, duration).SetUpdate(true);
        ;
        await _splashScreenTween;
    }

    public async UniTask HidePanel()
    {
        _splashScreenTween.CheckAndEnd(false);
        _splashScreenTween = _rt.DOAnchorPosY(_panelHeight, ConstUIAnimation.SPLASH_SCREEN_ANIM_DUR).SetUpdate(true);
        ;
        await _splashScreenTween;
    }

#endregion

#region Private methods

    void OnUpdateProgress_handler(float progress) => _imgLoadingBar.fillAmount = Mathf.Clamp(progress, 0, 1);

    void OnUpdateLoadingTip_handler(string tip) => (_localizedLoadingProgress.StringReference["0"] as StringVariable)!.Value = tip;

#endregion
}
}