using Zenject;
using Source.MVP;
using UnityEngine;
using FMOD.Studio;
using StateMachine;
using Source.Audio;
using Source.Player;
using Source.UI.Gameplay;
using Source.StateMachine;
using Cysharp.Threading.Tasks;

namespace Source.Gameplay
{
public class GameplayMediator : MonoBehaviour, IGameplayMediator
{
#region Fields

    [SerializeField] GampelayEntryPoint gampelayEntryPoint;

    [Space]
    [SerializeField] GamePanelGameplay _gameplayView;
    [SerializeField] GameUIController _uiController;
    [SerializeField] PlayerFacadeGameplay _playerFacadeGameplay;
    [SerializeField] EnemiesSpawner _enemiesSpawner;

    IAudioService _audioService;
    GameProgressModel _gameProgressModel;
    StatesMachine _gameplayStateMachine;
    GameProgressPresenter _progressPresenter;

#endregion

#region Public methods
    
    [Inject]
    public void Construct(GameProgressModel gameProgressModel, IAudioService audioService)
    {
        _gameProgressModel = gameProgressModel;
        _audioService = audioService;
    }

    public async UniTask Init(GameObject canvasInputHandler)
    {
        _uiController.Init();
        _playerFacadeGameplay.Init(canvasInputHandler);
        _progressPresenter = new GameProgressPresenter(_gameplayView, _gameProgressModel);
        InitGameplayStateMachine();

        await _enemiesSpawner.Init();
    }

    public void Deinit()
    {
        _uiController.Deinit();
        _enemiesSpawner.Deinit();
        _progressPresenter.Deinit();
        _playerFacadeGameplay.Deinit();
        _audioService.PauseSnapshot.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void SetGameState(EGameplayState state)
    {
        switch (state)
        {
            case EGameplayState.Play:
                _gameplayStateMachine.Enter<StateGameplay_Play, bool>(false).GetAwaiter();
                break;
            case EGameplayState.NewGame:
                _gameplayStateMachine.Enter<StateGameplay_Play, bool>(true).GetAwaiter();
                break;
            case EGameplayState.Pause:
                _gameplayStateMachine.Enter<StateGameplay_Pause, bool>(false).GetAwaiter();
                break;
            case EGameplayState.Loose:
                _gameplayStateMachine.Enter<StateGameplay_Pause, bool>(true).GetAwaiter();
                break;
            case EGameplayState.Leave:
                gampelayEntryPoint.GoToMenu().GetAwaiter();
                break;
        }
    }

#endregion

#region Private methods

    void InitGameplayStateMachine()
    {
        _gameplayStateMachine = new StatesMachine();

        var states = new IState[]
        {
            new StateGameplay_Pause(_uiController, _enemiesSpawner, _playerFacadeGameplay, _progressPresenter, _audioService),
            new StateGameplay_Play(_uiController, _enemiesSpawner, _playerFacadeGameplay, _progressPresenter),
        };

        _gameplayStateMachine = new StatesMachine(states);
    }

#endregion
}
}