using TMPro;
using System;
using Helpers;
using Source.MVP;
using UnityEngine;
using DG.Tweening;
using IdleNumbers;
using Source.Audio;
using Helpers.Audio;
using UnityEngine.UI;
using Source.Gameplay;
using Cysharp.Threading.Tasks;

namespace Source.UI.Gameplay
{
public class GamePanelGameplay : GamePanel, IViewGameplay
{
#region Fields

    [Space]
    [SerializeField] ButtonBase _buttonPause;
    [SerializeField] Slider _sliderProgress;
    [SerializeField] TextMeshProUGUI _txtLvlCurrent;
    [SerializeField] TextMeshProUGUI _txtCoin;
    
    [Space]
    [SerializeField] ParticleSystem _newLvlFx_1;
    [SerializeField] ParticleSystem _newLvlFx_2;

    float _lvlTextAnimDur = .25f;
    Vector3 _lvlTextRTMaxSize = new(1.3f, 1.3f, 1);
    RectTransform _lvlTextRT;

    float _coinsAnimDur = .1f;
    Vector3 _coinsRTMaxSize = new(1.1f, 1.1f, 1);
    RectTransform _coinsRT;

    Tween _sliderTween;
    Tween _coinTween;
    Sequence _newLvlSequence;

#endregion

#region Public methods

    public override void Init()
    {
        _lvlTextRT = _txtLvlCurrent.rectTransform;
        _coinsRT = _txtCoin.rectTransform;
        
        _buttonPause.Init();
        _buttonPause.onClick.AddListener(OnClickPause_handler);
    }

    public override void Deinit()
    {
        base.Deinit();
        _coinTween.CheckAndEnd();
        _sliderTween.CheckAndEnd();
        _newLvlSequence.CheckAndEnd();
    }
    
    public void SetNewGameInfo(int lvl)
    {
        _sliderProgress.value = 0;
        _txtCoin.SetText($"0 {ConstSpriteAssets.SPRITE_TEXT_COIN}");
        _txtLvlCurrent.SetText(lvl.ToString());
    }

    public void OnChangeCoins_handler(IdleNumber collectedCoins)
    {
        _coinTween.CheckAndEnd();
        _coinTween = _coinsRT.DOScale(_coinsRTMaxSize, _coinsAnimDur).SetLoops(2, LoopType.Yoyo);
        _txtCoin.SetText($"{collectedCoins.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}");
    }

    public void OnChangeProgress_handler(float progress)
    {
        var fillAmount = progress / ConstGameplay.PROGRESS_TARGET;
        _sliderTween.CheckAndEnd();
        _sliderTween = _sliderProgress.DOValue(fillAmount, _lvlTextAnimDur);
    }

    public async UniTaskVoid PlayNewLvlAnimation(int newLvl, Action OnAnimationFinish)
    {
        _sliderTween.CheckAndEnd(false);
        _newLvlSequence.CheckAndEnd(false);

        _newLvlSequence = DOTween.Sequence()
                                 .Pause()
                                 .Append(_sliderProgress.DOValue(1, _lvlTextAnimDur))
                                 .Join(_lvlTextRT.DOScale(_lvlTextRTMaxSize, _lvlTextAnimDur).From(Vector3.one)
                                                 .SetLoops(2, LoopType.Yoyo)
                                                 .OnStart(() =>
                                                 {
                                                     FmodEvents.Instance.LvlUp.PlayOneShot();
                                                     PlayNewLvlEffects().Forget();
                                                 })
                                                 .OnComplete(() => _txtLvlCurrent.SetText($"{newLvl}")))
                                 .Append(_sliderProgress.DOValue(0, _lvlTextAnimDur * 2));

        using (InputManager.Instance.LockInputSystem())
            await _newLvlSequence.Play();

        OnAnimationFinish?.Invoke();
    }

#endregion
    
    void OnClickPause_handler() => GameplayMediator.SetGameState(EGameplayState.Pause);

    async UniTaskVoid PlayNewLvlEffects()
    {
        _newLvlFx_1.Play();
        await UniTask.Delay(TimeSpan.FromSeconds(.2f));
        _newLvlFx_2.Play();
    }
}
}