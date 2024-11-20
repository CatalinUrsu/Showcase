using System;
using IdleNumbers;
using Cysharp.Threading.Tasks;

namespace Source.MVP
{
public interface IViewGameplay
{
    void SetNewGameInfo(int lvl);
    void OnChangeProgress_handler(float progress);
    void OnChangeCoins_handler(IdleNumber collectedCoins);
    UniTaskVoid PlayNewLvlAnimation(int newLvl, Action OnAnimationFinish);
}
}