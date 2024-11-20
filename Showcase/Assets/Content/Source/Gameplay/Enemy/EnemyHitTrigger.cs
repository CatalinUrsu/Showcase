using System;
using UnityEngine;
using IdleNumbers;

namespace Source.Gameplay
{
public class EnemyHitTrigger : MonoBehaviour
{
    public event Action OnHitPlayer;
    public event Action<IdleNumber, Vector2> OnHit;

    public void OnHitPlayer_raise() => OnHitPlayer?.Invoke();
    public void OnHit_raise(IdleNumber damage, Vector2 impulseDirection) => OnHit?.Invoke(damage, impulseDirection);
}
}