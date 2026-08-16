using IdleNumbers;
using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/Enemy", fileName = "Enemy_", order = 1)]
public class EnemySO : ScriptableObject
{
    public IdleNumber Coin;
    public IdleNumber HP;
    public float RewardPoints;
    public Vector2 SpeedRadius;
    public Sprite EnemyAppearence;
}
}