using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Source.UI.Gameplay
{
public class GamePanel : MonoBehaviour
{
#region Fields

    [SerializeField] EGamePanels _ePanelType;
    [SerializeField] protected GamePanelAnimation _animation;

    public EGamePanels EPanelType => _ePanelType;

#endregion

#region Public methods

    public virtual void Init() { }

    public virtual void Deinit() => _animation.Deinit();

    public virtual async UniTask Show()
    {
        gameObject.SetActive(true);
        await _animation.Show();
    }

    public virtual async UniTask Hide()
    {
        await _animation.Hide();
        gameObject.SetActive(false);
    }

#endregion
}
}