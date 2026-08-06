using Cysharp.Threading.Tasks;

namespace Source
{
public interface IGameUIFacade
{
    
    void Init();
    void Deinit();
    UniTask SelectPanel(EGamePanels panelType);
}
}