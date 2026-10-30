using UnityEngine;
using Unity.Cinemachine;
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

    public void SetFollow(ECinemachineState state, Transform target) => _cinemaCameras[state.ToString()].Camera.Follow = target;

    public void SetConfiner(ECinemachineState state, Collider2D bounds) =>
        _cinemaCameras[state.ToString()].Camera.GetComponent<CinemachineConfiner2D>().BoundingShape2D = bounds;
}
}