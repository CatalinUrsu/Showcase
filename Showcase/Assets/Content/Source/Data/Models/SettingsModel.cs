using R3;
using System;

namespace Source.Data
{
[Serializable]
public class SettingsModel : ISettingsModel
{
    // ---------- Runtime ----------
    public ReadOnlyReactiveProperty<bool> SoundRef => Sound;
    public ReadOnlyReactiveProperty<bool> MusicRef => Music;
    public ReadOnlyReactiveProperty<float> SoundVolumeRef => SoundVolume;
    public ReadOnlyReactiveProperty<float> MusicVolumeRef => MusicVolume;
    public ReadOnlyReactiveProperty<int> LocaleIdxRef => LocaleIdx;

    // ---------- Serialized ----------
    public ReactiveProperty<bool> Sound { get; private set; }
    public ReactiveProperty<bool> Music { get; private set; }
    public ReactiveProperty<float> SoundVolume { get; private set; }
    public ReactiveProperty<float> MusicVolume { get; private set; }
    public ReactiveProperty<int> LocaleIdx { get; private set; }
}
}