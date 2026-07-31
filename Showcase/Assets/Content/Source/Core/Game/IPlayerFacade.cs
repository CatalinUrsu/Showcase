using Cysharp.Threading.Tasks;

namespace Source
{
public interface IPlayerFacade
{
    void Init();
    void Deinit();
    void ToggleControl(bool enable);
    UniTask ShowPlayer();
}
}