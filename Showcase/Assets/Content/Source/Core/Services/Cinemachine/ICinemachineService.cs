using UnityEngine;
using System.Collections.Generic;

namespace Source
{
public interface ICinemachineService
{
    IReadOnlyDictionary<string, StateDrivenCameraData> CinemaCameras { get; }

    void RegisterStateDrivenAnimator(Animator animator);
    void AddStateDrivenCameraData(string state, StateDrivenCameraData data);
    void ChangeState(string state);
    void SetFollow(ECinemachineState state, Transform target);
    void SetConfiner(ECinemachineState state, Collider2D bounds);
}
}