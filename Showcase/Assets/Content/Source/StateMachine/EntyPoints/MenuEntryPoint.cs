using System;
using Helpers;
using UnityEngine;
using Helpers.StateMachine;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
{
public class MenuEntryPoint : MonoBehaviour, IEntryPoint
{
#region Fields

    [SerializeField] BankLoader _bankLoader;
    [SerializeField] MenuMediator _menuMediator;

    StatesMachine _statesMachine;

#endregion

#region Public methods
    
    public async UniTask Init(StatesMachine statesMachine, Action<float> onUpdateProgress)
    {
        _statesMachine = statesMachine;
        
        await _bankLoader.Init();
        await _menuMediator.Init(onUpdateProgress);
        _menuMediator.OnClickStartGame += () => StartGame().Forget();
    }

    public async UniTask Enter() => await _menuMediator.ShowPlayer();

    /// <summary>
    /// Deinit content on scene. In this project there's no cases when need to wait something, but it may be in other projects (ex: save data, network operation).
    /// </summary>
    public async UniTask Exit()
    {
        _menuMediator.Deinit();
        _bankLoader.Deinit();

        await UniTask.CompletedTask;
    }

#endregion
    
    async UniTaskVoid StartGame()
    {
        using (InputManager.Instance.LockInputSystem())
            await _statesMachine.Enter<StateGameplay>();
    }
}
}