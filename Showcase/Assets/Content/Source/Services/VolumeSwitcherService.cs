using Helpers;
using DG.Tweening;
using UnityEngine.Rendering;
using System.Collections.Generic;

namespace Source.Services
{
public class VolumeSwitcherService : IVolumeSwitcherService
{
    const float TRANSITION_DURATION = 0.5f;

    Tween _fadeInTween;
    Tween _fadeOutTween;
    Volume _currentVolume;
    readonly Dictionary<EVolumeState, Volume> _volumes = new();

    public VolumeSwitcherService(VolumeByState[] volumes)
    {
        foreach (var volumeByState in volumes)
            _volumes[volumeByState.State] = volumeByState.Volume;
    }

    public void ChangeState(EVolumeState state)
    {
        if (state == EVolumeState.Global)
            DisableVolume();
        else
            SwitchToVolume(state);
    }
    
    void DisableVolume()
    {
        if (_currentVolume)
            FadeOut(_currentVolume);
        _currentVolume = null;
    }
    
    void SwitchToVolume(EVolumeState state)
    {
        if (!_volumes.TryGetValue(state, out var newVolume) || newVolume == _currentVolume)
        {
            UnityEngine.Debug.LogWarning($"[VolumeSwitcherService] No volume found for state {state} or already active");
            return;
        }
        
        if (_currentVolume)
            FadeOut(_currentVolume);
        
        _currentVolume = newVolume;
        FadeIn(_currentVolume);
    }

    void FadeIn(Volume volume)
    {
        _fadeInTween.CheckAndEnd();
        _fadeOutTween = DOVirtual.Float(volume.weight, 1, TRANSITION_DURATION, weight => volume.weight = weight)
                                 .SetEase(Ease.InOutSine)
                                 .SetUpdate(true);
    }

    void FadeOut(Volume volume)
    {
        _fadeOutTween.CheckAndEnd();
        _fadeOutTween = DOVirtual.Float(volume.weight, 0, TRANSITION_DURATION, weight => volume.weight = weight)
                                 .SetEase(Ease.InOutSine)
                                 .SetUpdate(true);
    }
}
}