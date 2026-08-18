using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class MenuUIFacade : MonoBehaviour, IMenuUIFacade
{
#region Fields
    
    [SerializeField] TabsGroupMenu _tabsGroup;
    [SerializeField] MenuStartButton _menuStartButton;

    public event Action OnClickStartGame;

#endregion

#region Public methods

    public async UniTask Init(Action<float> onUpdateProgress)
    {
        _menuStartButton.Init(this);
        await _tabsGroup.Init(onUpdateProgress);
    }

    public async UniTask Deinit()
    {
        _menuStartButton.Deinit();
        await _tabsGroup.Deinit();
    }

    public void ClickStartGame_raise() => OnClickStartGame?.Invoke();

#endregion
}
}