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
        if (!_volumes.TryGetValue(state, out var newVolume) || newVolume == _currentVolume)
            return;
        
        if (state == EVolumeState.Global)
        {
            FadeOut(_currentVolume);
            _currentVolume = null;
            return;
        }

        FadeOut(_currentVolume);
        _currentVolume = newVolume;
        FadeIn(_currentVolume);
    }

    void FadeIn(Volume volume)
    {
        _fadeInTween.CheckAndEnd();
        _fadeInTween = DOTween.To(() => volume.weight, w => volume.weight = w, 1f, TRANSITION_DURATION)
                              .SetEase(Ease.InOutSine)
                              .SetTarget(volume);
    }

    void FadeOut(Volume volume)
    {
        _fadeOutTween.CheckAndEnd();
        _fadeOutTween = DOTween.To(() => volume.weight, w => volume.weight = w, 0f, TRANSITION_DURATION)
                               .SetEase(Ease.InOutSine)
                               .SetTarget(volume)
                               .OnComplete(() => volume.enabled = false);
    }
}
}