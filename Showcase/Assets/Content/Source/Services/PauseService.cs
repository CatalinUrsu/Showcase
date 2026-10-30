using UnityEngine;
using FMOD.Studio;

namespace Source.Services
{
public class PauseService : IPauseService
{
    readonly IAudioService _audioService;
    readonly IVolumeSwitcherService _volumeSwitcherService;

    public PauseService(IAudioService audioService, IVolumeSwitcherService volumeSwitcherService)
    {
        _audioService = audioService;
        _volumeSwitcherService = volumeSwitcherService;
    }

    public void Pause(EVolumeState volumeState, bool freezeTime)
    {
        if (freezeTime)
            Time.timeScale = 0;

        _audioService.PauseSnapshot.start();
        _volumeSwitcherService.ChangeState(volumeState);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _audioService.PauseSnapshot.stop(STOP_MODE.ALLOWFADEOUT);
        _volumeSwitcherService.ChangeState(EVolumeState.Global);
    }
}
}
