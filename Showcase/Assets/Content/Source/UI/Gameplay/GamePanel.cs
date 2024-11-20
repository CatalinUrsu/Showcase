using Zenject;
using UnityEngine;
using Source.Gameplay;
using Cysharp.Threading.Tasks;

namespace Source.UI.Gameplay
{
public class GamePanel : MonoBehaviour
{
#region Fields

    [SerializeField] protected GamePanelAnimation _animation;

    protected IGameplayMediator GameplayMediator;

#endregion

#region Public methods

    [Inject]
    public void Construct(IGameplayMediator gameplayMediator) => GameplayMediator = gameplayMediator;

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