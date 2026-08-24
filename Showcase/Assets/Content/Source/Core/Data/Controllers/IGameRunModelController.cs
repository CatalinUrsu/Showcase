using System;
using IdleNumbers;

namespace Source
{
public interface IGameRunModelController
{
    IGameRunModel IModel { get; }
    event Action OnClickPause;
    event Action OnClickGoHome;
    event Action OnContinueGame;
    event Action OnRestartRun;
    event Action OnPlayerLoose;

    void StartRun();
    void KillPlayer();
    
    void AddCoinsReward(float rewardPoints, IdleNumber coins);
    
    void OnClickPause_raise();
    void OnClickGoHome_raise();
    void OnClickContinue_raise();
    void OnClickRestartRun_raise();

    void StartNewLvlAnim();
    void FinishNewLvlAnim();
}
}