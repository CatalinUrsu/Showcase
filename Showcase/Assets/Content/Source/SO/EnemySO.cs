using IdleNumbers;
using UnityEngine;

namespace Source.SO.Enemy
{
[CreateAssetMenu(menuName = "SO/Enemy", fileName = "Enemy_")]
public class EnemySO : ScriptableObject
{
    public IdleNumber Coin;
    public IdleNumber HP;
    public float RewardPoints;
    public Vector2 SpeedRadius;
    public Sprite EnemyAppearence;
}
}