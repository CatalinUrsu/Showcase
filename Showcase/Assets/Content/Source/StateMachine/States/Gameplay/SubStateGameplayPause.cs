using Source.MVP;
using UnityEngine;
using FMOD.Studio;
using Source.Audio;
using Helpers.Audio;
using Source.Player;
using Source.Session;
using Source.Gameplay;
using Source.UI.Gameplay;
using Cysharp.Threading.Tasks;

namespace Source.StateMachine
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
                               GameProgressPresenter progressPresenter,
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
        FmodEvents.Instance.PopupClose.PlayOneShot();
        
        if (_isLoose)
            await _uiController.HidePanel(EGamePanels.Loose);
        else
            await _uiController.HidePanel(EGamePanels.Pause);
    }

#endregion

#region Private methods

    async UniTask OpenLooseMenu()
    {
        SessionManager.Current.Save(ESaveFileType.Progress);
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
        FmodEvents.Instance.PopupOpen.PlayOneShot();
        _audioService.PauseSnapshot.start();
    }

#endregion
}
}