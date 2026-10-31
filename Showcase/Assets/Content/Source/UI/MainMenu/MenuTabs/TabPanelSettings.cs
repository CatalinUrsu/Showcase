using R3;
using Zenject;
using Source.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class TabPanelSettings : TabsPanelMenu, ISettingsView
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

    SettingsPresenter.Factory _presenterFactory;
    SettingsPresenter _presenter;
    
    public Observable<float> SoundVolumeChanged => _sliderSound.onValueChanged.AsObservable();
    public Observable<float> MusicVolumeChanged => _sliderMusic.onValueChanged.AsObservable();
    public Observable<Unit> SoundClicked => _buttonSound.Btn.OnClickAsObservable();
    public Observable<Unit> MusicClicked => _buttonMusic.Btn.OnClickAsObservable();
    public Observable<Unit> LanguageClicked => _buttonLanguage.Btn.OnClickAsObservable();

#endregion

#region Public methods

    [Inject]
    public void Construct(SettingsPresenter.Factory presenterFactory) => _presenterFactory = presenterFactory;

    public override async UniTask Init(CancellationToken cancelToken, object config = null)
    {
        await base.Init(cancelToken, config);

        _sliderSound.maxValue = 1;
        _sliderMusic.maxValue = 1;
        _buttonSound.Init();
        _buttonMusic.Init();
        _buttonLanguage.Init();

        _itemsAnims = _settingParts.Select(part => part.GetComponent<MenuItemAnimation>()).ToList();
        SetShowSequence();

        _presenter = _presenterFactory.Create(this);
        await SetLayoutComponents(cancelToken);
    }
    
    public void ShowSound(bool isEnabled, float volume)
    {
        _sliderSound.SetValueWithoutNotify(volume);
        _buttonSound.OnToggleChange_handler(isEnabled);
    }

    public void ShowMusic(bool isEnabled, float volume)
    {
        _sliderMusic.SetValueWithoutNotify(volume);
        _buttonMusic.OnToggleChange_handler(isEnabled);
    }

#endregion

#region Private methods

    void OnDestroy() => _presenter?.Dispose();

#endregion
}
}
