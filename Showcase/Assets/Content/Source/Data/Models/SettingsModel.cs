using R3;
using System;
using System.Text.Json.Serialization;

namespace Source.Data
{
[Serializable]
public class SettingsModel : ISettingsModel
{
    // ---------- Runtime ----------
    [JsonIgnore] public ReadOnlyReactiveProperty<bool> SoundRef => Sound;
    [JsonIgnore] public ReadOnlyReactiveProperty<bool> MusicRef => Music;
    [JsonIgnore] public ReadOnlyReactiveProperty<float> SoundVolumeRef => SoundVolume;
    [JsonIgnore] public ReadOnlyReactiveProperty<float> MusicVolumeRef => MusicVolume;
    [JsonIgnore] public ReadOnlyReactiveProperty<int> LocaleIdxRef => LocaleIdx;

    // ---------- Serialized ----------
    [JsonInclude] public ReactiveProperty<bool> Sound { get; private set; } = new(true);
    [JsonInclude] public ReactiveProperty<bool> Music { get; private set; } = new(true);
    [JsonInclude] public ReactiveProperty<float> SoundVolume { get; private set; } = new(1);
    [JsonInclude] public ReactiveProperty<float> MusicVolume { get; private set; } = new(1);
    [JsonInclude] public ReactiveProperty<int> LocaleIdx { get; private set; } = new();
}
}