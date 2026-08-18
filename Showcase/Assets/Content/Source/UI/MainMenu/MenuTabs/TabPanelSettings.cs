using R3;
using System;
using Zenject;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Settings;

namespace Source.UI
{
public class TabPanelSettings : TabsPanelMenu
{
#region Fields

    [Space]
    [SerializeField] ButtonToggle _buttonSound;
    [SerializeField] ButtonToggle _buttonMusic;
    [SerializeField] Slider _sliderSound;
    [SerializeField] Slider _sliderMusic;

    [Space]
    [SerializeField] ButtonBase _buttonLanguage;

    [Space]
    [SerializeField] Transform[] _settingParts;

    TimeSpan _delayTimeSpan = TimeSpan.FromSeconds(2);
    CancellationTokenSource _saveTimerCTS;
    ISettingsModelController _settingsModelController;
    ISettingsModel _settingsModel;
    IAudioService _audioService;
    ISessionService _sessionService;

#endregion

#region Public methods

    [Inject]
    public void Construct(IAudioService audioService, ISessionService sessionService, ISettingsModelController settingsModelController)
    {
        _audioService = audioService;
        _sessionService = sessionService;
        _settingsModelController = settingsModelController;
        _settingsModel = settingsModelController.IModel;
    }

    public override async UniTask Init(CancellationToken cancelToken, object config = null)
    {
        await base.Init(cancelToken, config);

        _saveTimerCTS = new CancellationTokenSource();
        InitElements();
        InitPanelAnimation();
        SubscribeSettingsEvents();

        await SetLayoutComponents(cancelToken);
    }

#endregion

#region Private methods

    void OnDestroy()
    {
        _saveTimerCTS.Cancel();
        _sessionService.Save(ESaveFileType.Settings);
    }

    void InitElements()
    {
        // Set Sliders
        _sliderSound.maxValue = 1;
        _sliderMusic.maxValue = 1;
        _sliderSound.onValueChanged.AddListener(value => _settingsModelController.SetSoundVolume(value));
        _sliderMusic.onValueChanged.AddListener(value => _settingsModelController.SetMusicVolume(value));

        // Set buttons
        _buttonSound.Init();
        _buttonMusic.Init();
        _buttonLanguage.Init();
        _buttonSound.Btn.onClick.AddListener(() => _settingsModelController.SetSoundEnabled(!_settingsModel.SoundRef.CurrentValue));
        _buttonMusic.Btn.onClick.AddListener(() => _settingsModelController.SetMusicEnabled(!_settingsModel.MusicRef.CurrentValue));
        _buttonLanguage.Btn.onClick.AddListener(OnClickLanguage_handler);
        return;

        void OnClickLanguage_handler()
        {
            var newLocaleIdx = (_settingsModel.LocaleIdxRef.CurrentValue + 1) % LocalizationSettings.AvailableLocales.Locales.Count;

            _settingsModelController.SetLocaleIdx(newLocaleIdx);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[newLocaleIdx];
        }
    }

    void InitPanelAnimation()
    {
        _itemsAnims = _settingParts.Select(part => part.GetComponent<MenuItemAnimation>()).ToList();
        SetShowSequence();
    }

    void SubscribeSettingsEvents()
    {
        _settingsModel.SoundRef.Subscribe(OnChangeSoundToggle_handler).AddTo(this);
        _settingsModel.SoundVolumeRef.Subscribe(OnChangeSoundVolume_handler).AddTo(this);
        _settingsModel.MusicRef.Subscribe(OnChangeMusicToggle_handler).AddTo(this);
        _settingsModel.MusicVolumeRef.Subscribe(OnChangeMusicVolume_handler).AddTo(this);
        return;

        void OnChangeSoundToggle_handler(bool enable)
        {
            _sliderSound.value = enable ?
                _sliderSound.value == 0 ?
                    .5f :
                    _sliderSound.value :
                0;
            _buttonSound.OnToggleChange_handler(enable);

            SetSaveTimer().Forget();
        }

        void OnChangeSoundVolume_handler(float value)
        {
            _sliderSound.value = value;
            _settingsModelController.SetSoundEnabled(_sliderSound.value > 0);

            _audioService.SetSoundVolume(_sliderSound.value);
            SetSaveTimer().Forget();
        }

        void OnChangeMusicToggle_handler(bool enable)
        {
            _sliderMusic.value = enable ?
                _sliderMusic.value == 0 ?
                    .5f :
                    _sliderMusic.value :
                0;
            _buttonMusic.OnToggleChange_handler(enable);

            SetSaveTimer().Forget();
        }

        void OnChangeMusicVolume_handler(float value)
        {
            _sliderMusic.value = value;
            _settingsModelController.SetMusicEnabled(_sliderMusic.value > 0);

            _audioService.SetMusicVolume(_sliderMusic.value);
            SetSaveTimer().Forget();
        }

        async UniTaskVoid SetSaveTimer()
        {
            _saveTimerCTS.Cancel();
            _saveTimerCTS = new CancellationTokenSource();
            await UniTask.Delay(_delayTimeSpan, cancellationToken: _saveTimerCTS.Token);
            _sessionService.Save(ESaveFileType.Settings);
        }
    }

#endregion
}
}