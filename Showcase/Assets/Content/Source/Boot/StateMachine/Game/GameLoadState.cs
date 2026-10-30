using System;
using Helpers;
using UnityEngine;
using System.Threading;
using Helpers.Services;
using Cysharp.Threading.Tasks;

namespace Source.Boot
{
/// <summary>
/// Loads gameplay scene and prepares camera. On failure unloads scene itself, otherwise its entry point stays in registry.
/// </summary>
public class GameLoadState : IStateEnter
{
#region Fields

    public StatesMachine StatesMachine { get; set; }

    readonly IInputLock _inputLock;
    readonly ISceneLifecycleService _sceneLifecycleService;
    readonly ICinemachineService _cinemachineService;
    readonly EntryPointRegistry _entryPointRegistry;
    readonly ILoadingEntryPoint _loadingEntryPoint;

#endregion

#region Public methods

    public GameLoadState(ISceneLifecycleService sceneLifecycleService,
                         ICinemachineService cinemachineService,
                         EntryPointRegistry entryPointRegistry,
                         ILoadingEntryPoint loadingEntryPoint,
                         IInputLock inputLock)
    {
        _inputLock = inputLock;
        _sceneLifecycleService = sceneLifecycleService;
        _cinemachineService = cinemachineService;
        _entryPointRegistry = entryPointRegistry;
        _loadingEntryPoint = loadingEntryPoint;
    }

    public async UniTask Enter(CancellationToken cancellationToken)
    {
        try
        {
            using (_inputLock.LockInputSystem())
            {
                var gameEntryPoint = await LoadGameScene();

                _cinemachineService.SetConfiner(ECinemachineState.Game, gameEntryPoint.GameCameraBounds);
                _cinemachineService.ChangeState(nameof(ECinemachineState.Game));
                await _loadingEntryPoint.SplashScreen.Hide();
                gameEntryPoint.EnemiesSpawner.ToggleSpawning(true);
            }
        }
        catch
        {
            await UnloadGameScene();
            throw;
        }

        StatesMachine.Enter<GameStartState>();
    }

    public UniTask Exit() => UniTask.CompletedTask;

#endregion

#region Private methods

    UniTask<IGameplayEntryPoint> LoadGameScene()
    {
        var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.GAME_SCENE)
                              .SetTip("Load Gameplay Scene")
                              .SetIsAddressable(true)
                              .SetActiveOnLoad(true)
                              .Build();

        return _sceneLifecycleService.LoadAndInitScene<IGameplayEntryPoint>(sceneLoadParams);
    }

    async UniTask UnloadGameScene()
    {
        try
        {
            var gameplay = _entryPointRegistry.Get<IGameplayEntryPoint>();
            await _sceneLifecycleService.DeinitAndUnloadScene(ConstSceneNames.GAME_SCENE, gameplay);
        }
        catch (Exception e)
        {
            // Original exception is rethrown by caller, this one is only logged
            Debug.LogException(e);
        }
    }

#endregion
}
}