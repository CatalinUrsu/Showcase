using System;
using Helpers;
using Zenject;
using Source.UI;
using UnityEngine;
using NUnit.Framework;
using System.Collections;
using Source.UI.Gameplay;
using UnityEngine.TestTools;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Source.Tests
{
/// <summary>
/// Real game flow: Init -> Menu -> Game -> Pause -> Menu. Catches broken bindings, scenes and state transitions.
/// </summary>
public class FlowSmokeTests
{
    static readonly TimeSpan STEP_TIMEOUT = TimeSpan.FromSeconds(15);

    IInputManager _inputManager;

    [UnityTest]
    public IEnumerator Flow_InitMenuGameMenu_EachStepReady() => UniTask.ToCoroutine(async () =>
    {
        SceneManager.LoadScene(ConstSceneNames.INIT_SCENE);

        await WaitUntil(() => ProjectContext.HasInstance, "ProjectContext");
        _inputManager = ProjectContext.Instance.Container.Resolve<IInputManager>();
        var gameCommands = ProjectContext.Instance.Container.Resolve<IGameCommands>();

        await WaitReady<MenuUIFacade>("Menu");
        Object.FindFirstObjectByType<MenuUIFacade>().ClickStartGame_raise();

        await WaitReady<GameUIFacade>("Game");
        // Pause state subscribes to commands synchronously in Send, GoHome waits in transition queue until pause is shown
        gameCommands.Send(EGameCommand.Pause);
        gameCommands.Send(EGameCommand.GoHome);

        await WaitReady<MenuUIFacade>("Menu after game");
    });

    // Step is ready when its scene object exists and transition finished (input unlocked)
    async UniTask WaitReady<T>(string step) where T : Object =>
        await WaitUntil(() => Object.FindFirstObjectByType<T>() != null && !_inputManager.IsLocked, step);

    static async UniTask WaitUntil(Func<bool> condition, string step)
    {
        try
        {
            await UniTask.WaitUntil(condition).Timeout(STEP_TIMEOUT);
        }
        catch (TimeoutException)
        {
            Assert.Fail($"Step '{step}' is not ready after {STEP_TIMEOUT.TotalSeconds} s");
        }
    }
}
}
