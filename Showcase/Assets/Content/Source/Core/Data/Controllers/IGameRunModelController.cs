using IdleNumbers;

namespace Source
{
public interface IGameRunModelController
{
    IGameRunModel IModel { get; }
    bool CanPause { get; }

    void StartRun();
    void KillPlayer();
    
    void AddCoinsReward(float rewardPoints, IdleNumber coins);
    
    void StartNewLvlAnim();
    void FinishNewLvlAnim();
}
}