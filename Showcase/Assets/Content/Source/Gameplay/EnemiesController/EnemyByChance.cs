using System;
using Source.SO.Enemy;

namespace Source.Gameplay
{
[Serializable]
public class EnemyByChance
{
    public int Chance;
    public EnemySO EnemySO;

    public int MinChance { get; set; }
    public int MaxChance{ get; set; }
}
}