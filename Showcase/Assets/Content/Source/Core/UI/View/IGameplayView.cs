using IdleNumbers;
using Cysharp.Threading.Tasks;

namespace Source
{
public interface IGameplayView
{
    void SetNewGameInfo(int lvl);
    void SetProgressSlider(float normalizedProgress);
    void SetCollectedCoins(IdleNumber collectedCoins);
    UniTask PlayNewLvlAnimation(int newLvl);
}
}