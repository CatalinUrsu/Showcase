using Cysharp.Threading.Tasks;

namespace Source
{
public interface IEnemiesController
{
    UniTask Init();
    void Deinit();
    void ToggleSpawning(bool enabe);
}
}