using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source
{
public interface IPlayerFacade
{
    Transform Transform { get; }
    
    void Init();
    void Deinit();
    UniTask ShowPlayer();
    void ToggleControl(bool enable);
}
}