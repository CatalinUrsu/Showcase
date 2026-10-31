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

    public void SetSoundEnabled(bool value) => _sound.Value = value;

    public void SetMusicEnabled(bool value) => _music.Value = value;

    public void SetSoundVolume(float value) => _soundVolume.Value = Mathf.Clamp01(value);

    public void SetMusicVolume(float value) => _musicVolume.Value = Mathf.Clamp01(value);

    public void SetLocaleIdx(int value) => _localeIdx.Value = Mathf.Max(0, value);

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
