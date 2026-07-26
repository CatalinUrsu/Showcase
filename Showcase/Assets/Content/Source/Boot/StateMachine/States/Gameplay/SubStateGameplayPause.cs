using Cysharp.Threading.Tasks;
using FMOD.Studio;
using Source.Data;
using Source.Gameplay;
using Source.Player;
using Source.Services;
using Source.UI.Gameplay;
using UnityEngine;

namespace Source.Boot
{
public class SubStateGameplayPause : SubStateGameplay
{
#region Fields

    bool _isLoose;
    readonly IAudioService _audioService;

#endregion

#region Public methods

    public SubStateGameplayPause(GameUIController uiController,
                               EnemiesSpawner enemiesSpawner,
                               PlayerFacadeGameplay playerFacadeGameplay,
                               GameRunPresenter progressPresenter,
                               IAudioService audioService)
        : base(uiController, enemiesSpawner, playerFacadeGameplay, progressPresenter)
    {
        _audioService = audioService;
    }

    public override async UniTask Enter(bool payload)
    {
        _isLoose = payload;
        if (_isLoose)
            await OpenLooseMenu();
        else
            await OpenPauseMenu();
    }

    public override async UniTask Exit()
    {
        _audioService.PauseSnapshot.stop(STOP_MODE.ALLOWFADEOUT);
        FmodEventsSo.Instance.PopupClose.PlayOneShot();
        
        if (_isLoose)
            await _uiController.HidePanel(EGamePanels.Loose);
        else
            await _uiController.HidePanel(EGamePanels.Pause);
    }

#endregion

#region Private methods

    async UniTask OpenLooseMenu()
    {
        SessionService.Current.Save(ESaveFileType.Progress);
        _progressPresenter.Deinit();

        await _uiController.HidePanel(EGamePanels.Game);

        PlayPauseAudio();
        await _uiController.ShowPanel(EGamePanels.Loose);
    }

    async UniTask OpenPauseMenu()
    {
        Time.timeScale = 0;
        _enemiesSpawner.ToggleSpawning(false);
        _playerFacadeGameplay.ToggleControl(false);

        await _uiController.HidePanel(EGamePanels.Game);

        PlayPauseAudio();
        await _uiController.ShowPanel(EGamePanels.Pause);
    }

    void PlayPauseAudio()
    {
        FmodEventsSo.Instance.PopupOpen.PlayOneShot();
        _audioService.PauseSnapshot.start();
    }

#endregion
}
}