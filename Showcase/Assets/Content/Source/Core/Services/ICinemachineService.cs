using UnityEngine;
using Unity.Cinemachine;

namespace Source
{
public interface ICinemachineService
{
    void RegisterAnimator(Animator animator);
    void RegisterCinemachineGame(CinemachineCamera cinemachineCamera);
    void SetCinemachineGame(Transform target, Collider2D boundCollider);
    void ChangeState(ECinemachineState state);
}
}