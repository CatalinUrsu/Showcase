using Cysharp.Threading.Tasks;

namespace Source
{
public interface IEnemiesSpawner
{
    UniTask Init();
    void Deinit();
    void ToggleSpawning(bool enabe);
}
}