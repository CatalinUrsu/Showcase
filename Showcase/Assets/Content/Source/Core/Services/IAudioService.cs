using Zenject;
using FMOD.Studio;

namespace Source
{
public interface IAudioService
{
    EventInstance MusicInstance { get; }
    EventInstance FlyInstance { get; }
    EventInstance PauseSnapshot { get; }

    void Init(DiContainer projectContainer);
    void SetSoundVolume(float volume);
    void SetMusicVolume(float volume);
}
}
