using System;
using Cysharp.Threading.Tasks;
using Helpers;
using Helpers.Audio;
using Helpers.Services;
using Source.Services;
using UnityEngine;

namespace Source.Boot
{
public class GameplayEntryPoint : MonoBehaviour
{
#region Fields

    [SerializeField] Canvas _canvasInputHandler;
    [SerializeField] GameplayMediator _gameplayMediator;
    [SerializeField] BankLoader _bankLoader;

    StatesMachine _statesMachine;

#endregion

#region Public methods

    public async UniTask Init(StatesMachine statesMachine, Action<float> onUpdateProgress)
    {
        _statesMachine = statesMachine;
        InputManager.Instance.OnToggleInputLock += OnToggleInputLock_handler;

        await _bankLoader.Init();
        await UniTask.WhenAll(_gameplayMediator.Init(_canvasInputHandler.gameObject));

        onUpdateProgress.Invoke(1);
    }

    public async UniTask Enter()
    {
        _gameplayMediator.SetGameState(EGameplayState.NewGame);
        await UniTask.CompletedTask;
    }

    public async UniTask Exit()
    {
        _gameplayMediator.Deinit();
        _bankLoader.Deinit();

        Time.timeScale = 1;
        SessionService.Current.Save(ESaveFileType.Progress);
        InputManager.Instance.OnToggleInputLock -= OnToggleInputLock_handler;

        await UniTask.CompletedTask;
    }

    public async UniTask GoToMenu()
    {
        using (InputManager.Instance.LockInputSystem())
            await _statesMachine.Enter<StateMenu>();
    }

#endregion

#region Private methods

    void OnToggleInputLock_handler(bool isLock) => _canvasInputHandler.overrideSorting = isLock;

#endregion
}
}