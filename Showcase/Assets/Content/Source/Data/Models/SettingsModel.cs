using R3;
using UnityEngine;

namespace Source.Data
{
public class SettingsModel
{
#region Fields

    public ReadOnlyReactiveProperty<bool> Sound => _sound;
    public ReadOnlyReactiveProperty<bool> Music => _music;
    public ReadOnlyReactiveProperty<float> SoundVolume => _soundVolume;
    public ReadOnlyReactiveProperty<float> MusicVolume => _musicVolume;
    public ReadOnlyReactiveProperty<int> LocaleIdx => _localeIdx;

    readonly ReactiveProperty<bool> _sound;
    readonly ReactiveProperty<bool> _music;
    readonly ReactiveProperty<float> _soundVolume;
    readonly ReactiveProperty<float> _musicVolume;
    readonly ReactiveProperty<int> _localeIdx;

    const float VOLUME_ON_ENABLE = .5f;

#endregion

#region Public methods

    public SettingsModel(SettingsSaveData data)
    {
        _sound = new ReactiveProperty<bool>(data.Sound);
        _music = new ReactiveProperty<bool>(data.Music);
        _soundVolume = new ReactiveProperty<float>(data.SoundVolume);
        _musicVolume = new ReactiveProperty<float>(data.MusicVolume);
        _localeIdx = new ReactiveProperty<int>(data.LocaleIdx);
    }

    // Volume is the source of truth, enabled flag is derived from it
    public void SetSoundVolume(float value)
    {
        _soundVolume.Value = Mathf.Clamp01(value);
        _sound.Value = _soundVolume.Value > 0;
    }

    public void SetMusicVolume(float value)
    {
        _musicVolume.Value = Mathf.Clamp01(value);
        _music.Value = _musicVolume.Value > 0;
    }

    public void ToggleSound() => SetSoundVolume(_sound.Value ? 0 : VOLUME_ON_ENABLE);

    public void ToggleMusic() => SetMusicVolume(_music.Value ? 0 : VOLUME_ON_ENABLE);

    public void SetNextLocale(int localesCount) => _localeIdx.Value = (_localeIdx.Value + 1) % localesCount;

    public SettingsSaveData ToSaveData()
    {
        return new SettingsSaveData
        {
            Sound = _sound.Value,
            Music = _music.Value,
            SoundVolume = _soundVolume.Value,
            MusicVolume = _musicVolume.Value,
            LocaleIdx = _localeIdx.Value
        };
    }

#endregion
}
}
