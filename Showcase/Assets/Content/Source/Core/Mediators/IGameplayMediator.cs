using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Source
{
public interface IGameplayMediator
{
    UniTask Init(GameObject canvasInputHandler);
    void Deinit();
    void SetGameState(EGameplayState state);
}
}