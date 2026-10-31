using R3;

namespace Source
{
public interface ISettingsView
{
    Observable<float> SoundVolumeChanged { get; }
    Observable<float> MusicVolumeChanged { get; }
    Observable<Unit> SoundClicked { get; }
    Observable<Unit> MusicClicked { get; }
    Observable<Unit> LanguageClicked { get; }

    void ShowSound(bool isEnabled, float volume);
    void ShowMusic(bool isEnabled, float volume);
}
}
