using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

namespace Source.Services
{
public class CinemachineService : ICinemachineService
{
    Animator _animator;
    CinemachineCamera _cinemachineCamGame;
    readonly Dictionary<ECinemachineState, int> _stateHashes = new()
    {
        { ECinemachineState.Menu, Animator.StringToHash("Menu") },
        { ECinemachineState.Game, Animator.StringToHash("Game") }
    };

    public void RegisterAnimator(Animator animator) => _animator = animator;
    
    public void RegisterCinemachineGame(CinemachineCamera cinemachineCamera) => _cinemachineCamGame = cinemachineCamera;
    
    public void SetCinemachineGame(Transform target, Collider2D boundCollider)
    {
        _cinemachineCamGame.Target.TrackingTarget = target;
        
    }

    public void ChangeState(ECinemachineState state) => _animator.Play(_stateHashes[state]);
}
}