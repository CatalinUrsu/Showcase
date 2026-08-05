using Cysharp.Threading.Tasks;

namespace Source
{
public interface IGameUIFacade
{
    
    void Init();
    void Deinit();
    UniTask ShowPanel(EGamePanels panelType);
    UniTask HidePanel(EGamePanels panelType);
}
}