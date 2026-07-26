using R3;
using System;
using Zenject;
using FMOD.Studio;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Data;
using UnityEngine.Localization.Settings;

namespace Source.UI
{
public class TabPanelSettings : MenuTabsPanel
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

    SettingsModel _settingsModel;
    TimeSpan _delayTimeSpan = TimeSpan.FromSeconds(2);
    CancellationTokenSource _saveTimerCTS;

    [Inject] IAudioService _audioService;

#endregion
    
#region Public methods

    public override async UniTask InitContent(Action onFinishInit, EventInstance elementShowSound, CancellationTokenSource cts)
    {
        await base.InitContent(onFinishInit, elementShowSound, cts);
        
        _saveTimerCTS = new CancellationTokenSource();
        InitElements();
        InitPanelAnimation();
        SubscribeSettingsEvents();

        await SetLayoutComponents(onFinishInit, cts);
    }
    
#endregion

#region Private methods

    void OnDestroy()
    {
        _saveTimerCTS.Cancel();
        SessionService.Current.Save(ESaveFileType.Settings);
    }

    void InitElements()
    {
        _settingsModel = SessionService.Current.Settings;

        // Set Sliders
        _sliderSound.maxValue = 1;
        _sliderMusic.maxValue = 1;
        _sliderSound.onValueChanged.AddListener(value => _settingsModel.SoundVolume.Value = value);
        _sliderMusic.onValueChanged.AddListener(value => _settingsModel.MusicVolume.Value = value);

        // Set buttons
        _buttonSound.Init();
        _buttonMusic.Init();
        _buttonLanguage.Init();
        _buttonSound.onClick.AddListener(() => _settingsModel.Sound.Value = !_settingsModel.Sound.Value);
        _buttonMusic.onClick.AddListener(() => _settingsModel.Music.Value = !_settingsModel.Music.Value);
        _buttonLanguage.onClick.AddListener(OnClickLanguage_handler);

        void OnClickLanguage_handler()
        {
            _settingsModel.LocaleIdx.Value = (_settingsModel.LocaleIdx.Value + 1) % LocalizationSettings.AvailableLocales.Locales.Count;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_settingsModel.LocaleIdx.Value];
        }
    }

    void InitPanelAnimation()
    {
        _elemntsAnimations = _settingParts.Select(part => part.GetComponent<MenuElementAnimation>()).ToList();
    }

    void SubscribeSettingsEvents()
    {
        _settingsModel.Sound.Subscribe(OnChangeSoundToggle_handler).AddTo(this);
        _settingsModel.SoundVolume.Subscribe(OnChangeSoundVolume_handler).AddTo(this);
        _settingsModel.Music.Subscribe(OnChangeMusicToggle_handler).AddTo(this);
        _settingsModel.MusicVolume.Subscribe(OnChangeMusicVolume_handler).AddTo(this);
        
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
            _settingsModel.Sound.Value = _sliderSound.value > 0;
        
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
            _settingsModel.Music.Value = _sliderMusic.value > 0;
        
            _audioService.SetMusicVolume(_sliderMusic.value);
            SetSaveTimer().Forget();
        }

        async UniTaskVoid SetSaveTimer()
        {
            _saveTimerCTS.Cancel();
            _saveTimerCTS = new CancellationTokenSource();
            await UniTask.Delay(_delayTimeSpan, cancellationToken: _saveTimerCTS.Token);
            SessionService.Current.Save(ESaveFileType.Settings);
        }
    }

#endregion
}
}