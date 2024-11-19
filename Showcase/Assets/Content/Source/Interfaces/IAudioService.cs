using FMOD.Studio;

namespace Source.Audio
{
public interface IAudioService
{
    EventInstance MusicInstance { get; }
    EventInstance FlyInstance { get; }
    EventInstance PauseSnapshot { get; }

    void Init();
    void SetSoundVolume(float volume);
    void SetMusicVolume(float volume);
}
}
