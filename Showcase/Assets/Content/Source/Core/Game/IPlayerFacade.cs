using Cysharp.Threading.Tasks;

namespace Source
{
public interface IPlayerFacade
{
    void Init();
    void Deinit();
    UniTask ShowPlayer();
}
}