using System;
using IdleNumbers;

namespace Source
{
public interface IGameRunModelController : IDisposable
{
    IGameRunModel IModel { get; }

    void StartRun();

    void KillPlayer(bool alive);
    
    void AddKillReward(float rewardPoints, IdleNumber coins);
}
}