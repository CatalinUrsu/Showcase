using System;
using Cysharp.Threading.Tasks;

namespace Source
{
public interface IUIMenuFacade
{
    event Action OnClickStartGame;
    
    UniTask Init(Action<float> onUpdateProgress);
    void Deinit();

    void ClickStartGame_raise();
}
}