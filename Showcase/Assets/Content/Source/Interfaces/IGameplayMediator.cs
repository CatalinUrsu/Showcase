using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Source.Gameplay
{
public interface IGameplayMediator
{
    UniTask Init(GameObject canvasInputHandler);
    void Deinit();
    void SetGameState(EGameplayState state);
}
}