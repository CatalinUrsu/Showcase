using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class UIMenuFacade : MonoBehaviour, IUIMenuFacade
{
#region Fields
    
    [SerializeField] MenuTabsGroup _tabsGroup;
    [SerializeField] MenuStartButton _menuStartButton;

    public event Action OnClickStartGame;

#endregion

#region Public methods

    public async UniTask Init(Action<float> onUpdateProgress)
    {
        _menuStartButton.Init(this);
        await _tabsGroup.Init(onUpdateProgress);
    }

    public void Deinit()
    {
        _menuStartButton.Deinit();
    }

    public void ClickStartGame_raise() => OnClickStartGame?.Invoke();

#endregion
}
}