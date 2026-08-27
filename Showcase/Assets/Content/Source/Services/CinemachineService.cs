using UnityEngine;
using System.Collections.Generic;

namespace Source.Services
{
public class CinemachineService : ICinemachineService
{
    public IReadOnlyDictionary<string, StateDrivenCameraData> CinemaCameras => _cinemaCameras;

    Animator _animator;
    readonly Dictionary<string, StateDrivenCameraData> _cinemaCameras = new();

    public void RegisterStateDrivenAnimator(Animator animator) => _animator = animator;

    public void AddStateDrivenCameraData(string state, StateDrivenCameraData data) => _cinemaCameras[state] = data;

    public void ChangeState(string state) => _animator.Play(_cinemaCameras[state].StateHash);
}
}