using TMPro;
using System;
using Helpers;
using Zenject;
using UnityEngine;
using DG.Tweening;
using IdleNumbers;
using Source.Data;
using Helpers.Audio;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Source.UI.Gameplay
{
public class GamePanelGameplay : GamePanel, IGameplayView
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

    Vector3 _lvlTextRTMaxSize = new(1.3f, 1.3f, 1);
    RectTransform _lvlTextRT;
    FmodEventsSo _fmodEventsSo;

    Vector3 _coinsRTMaxSize = new(1.1f, 1.1f, 1);
    RectTransform _coinsRT;

    Tween _sliderTween;
    Tween _coinTween;
    Sequence _newLvlSequence;
    GameRunPresenter _gameRunPresenter;
    
    const float COINS_ANIM_DUR = .1f;
    const float LVL_TEXT_ANIM_DUR = .25f;

#endregion

#region Public methods

    [Inject]
    public void Construct(GameRunPresenter.Factory gameRunPresenterFactory, FmodEventsSo fmodEventsSo)
    {
        _gameRunPresenter = gameRunPresenterFactory.Create(this);
        _fmodEventsSo = fmodEventsSo;
    }

    public override void Init()
    {
        _lvlTextRT = _txtLvlCurrent.rectTransform;
        _coinsRT = _txtCoin.rectTransform;

        _buttonPause.Init();
        _buttonPause.Btn.onClick.AddListener(OnClickPause_handler);
    }

    public override void Deinit()
    {
        base.Deinit();
        _coinTween.CheckAndEnd();
        _sliderTween.CheckAndEnd();
        _newLvlSequence.CheckAndEnd();
        _gameRunPresenter.Dispose();
    }

    public void SetNewGameInfo(int lvl)
    {
        _sliderProgress.value = 0;
        _txtCoin.SetText($"0 {ConstSpriteAssets.SPRITE_TEXT_COIN}");
        _txtLvlCurrent.SetText(lvl.ToString());
    }

    public void SetCollectedCoins(IdleNumber collectedCoins)
    {
        _coinTween.CheckAndEnd();
        _coinTween = _coinsRT.DOScale(_coinsRTMaxSize, COINS_ANIM_DUR).SetLoops(2, LoopType.Yoyo);
        _txtCoin.SetText($"{collectedCoins.AsString()} {ConstSpriteAssets.SPRITE_TEXT_COIN}");
    }

    public void SetProgressSlider(float progress)
    {
        _sliderTween.CheckAndEnd();
        _sliderTween = _sliderProgress.DOValue(progress / ConstGameplay.PROGRESS_TARGET, LVL_TEXT_ANIM_DUR);
    }

    public async UniTask PlayNewLvlAnimation(int newLvl)
    {
        _sliderTween.CheckAndEnd(false);
        _newLvlSequence.FinishAndGetNew(false)
                       .Pause()
                       .Append(_sliderProgress.DOValue(1, LVL_TEXT_ANIM_DUR))
                       .Join(_lvlTextRT.DOScale(_lvlTextRTMaxSize, LVL_TEXT_ANIM_DUR).From(Vector3.one)
                                       .SetLoops(2, LoopType.Yoyo)
                                       .OnStart(PlayNewLvlAnimation)
                                       .OnComplete(() => _txtLvlCurrent.SetText($"{newLvl}")))
                       .Append(_sliderProgress.DOValue(0, LVL_TEXT_ANIM_DUR * 2));

        await _newLvlSequence.Play();
    }

#endregion

#region Private methods

    void OnClickPause_handler() => _gameRunPresenter.ClickPause();

    void PlayNewLvlAnimation()
    {
        _fmodEventsSo.LvlUp.PlayOneShot();
        PlayNewLvlEffects().Forget();
        return;

        async UniTaskVoid PlayNewLvlEffects()
        {
            _newLvlFx_1.Play();
            await UniTask.Delay(TimeSpan.FromSeconds(.2f));
            _newLvlFx_2.Play();
        }
    }

#endregion
}
}