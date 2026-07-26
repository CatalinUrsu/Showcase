using Cysharp.Threading.Tasks;
using Helpers;
using Source.Data;
using Source.Gameplay;
using Source.Player;
using Source.UI.Gameplay;
using UnityEngine;

namespace Source.Boot
{
public class SubStateGameplayPlay : SubStateGameplay
{
#region Public methods

    public SubStateGameplayPlay(GameUIController uiController, EnemiesSpawner enemiesSpawner, PlayerFacadeGameplay playerFacadeGameplay, GameRunPresenter progressPresenter)
        : base(uiController, enemiesSpawner, playerFacadeGameplay, progressPresenter) { }

    public override async UniTask Enter(bool payload)
    {
        if (payload)
            StartNewGame();
        else
            PlayGame().Forget();
        
        await UniTask.CompletedTask;
    }

    public override UniTask Exit()
    {
        return UniTask.CompletedTask;
    }

#endregion

#region Private methods

    void StartNewGame()
    {
        _progressPresenter.Init();
        _playerFacadeGameplay.ShowPlayer().Forget();
        PlayGame().Forget();
    }

    async UniTaskVoid PlayGame()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            await _uiController.ShowPanel(EGamePanels.Game);

            Time.timeScale = 1;
            _enemiesSpawner.ToggleSpawning(true);
            _playerFacadeGameplay.ToggleControl(true);
        }
    }

#endregion
}
}