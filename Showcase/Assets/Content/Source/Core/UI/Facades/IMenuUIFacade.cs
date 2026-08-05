using System;
using Cysharp.Threading.Tasks;

namespace Source
{
public interface IMenuUIFacade
{
    event Action OnClickStartGame;
    
    UniTask Init(Action<float> onUpdateProgress);
    UniTask Deinit();

    void ClickStartGame_raise();
}
}