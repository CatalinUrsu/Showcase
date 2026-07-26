using System;
using Cysharp.Threading.Tasks;
using IdleNumbers;

namespace Source
{
public interface IViewGameplay
{
    void SetNewGameInfo(int lvl);
    void OnChangeProgress_handler(float progress);
    void OnChangeCoins_handler(IdleNumber collectedCoins);
    UniTaskVoid PlayNewLvlAnimation(int newLvl, Action OnAnimationFinish);
}
}