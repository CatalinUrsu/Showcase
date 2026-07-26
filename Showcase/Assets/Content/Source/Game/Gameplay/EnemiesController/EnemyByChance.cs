using System;
using Source.Data;

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