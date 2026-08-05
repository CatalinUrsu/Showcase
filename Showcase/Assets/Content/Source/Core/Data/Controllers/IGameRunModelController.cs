using System;
using IdleNumbers;

namespace Source
{
public interface IGameRunModelController : IDisposable
{
    IGameRunModel IModel { get; }
    event Action OnClickPause;
    event Action OnClickGoHome;
    event Action OnClickReturnToGame;
    event Action OnPlayerLoose;

    void StartRun();
    void KillPlayer(bool alive);
    
    void AddCoinsReward(float rewardPoints, IdleNumber coins);
    
    void OnClickPause_raise();
    void OnClickGoHome_raise();
    void OnClickReturnToGame_raise();

    void StartNewLvlAnim();
    void FinishNewLvlAnim();
}
}