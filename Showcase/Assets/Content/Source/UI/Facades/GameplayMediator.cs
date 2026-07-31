using Cysharp.Threading.Tasks;
using FMOD.Studio;
using Helpers.Services;
using Source.Data;
using Source.Gameplay;
using Source.Player;
using Source.UI.Gameplay;
using UnityEngine;
using Zenject;

namespace Source.Boot
{
public class GameplayMediator : MonoBehaviour, IGameplayMediator
{
#region Fields

    [Space]
    [SerializeField] GamePanelGameplay _gameplayView;
    [SerializeField] GameUIController _uiController;
    [SerializeField] PlayerFacadeGameplay _playerFacadeGameplay;
    [SerializeField] EnemiesSpawner _enemiesSpawner;

    IAudioService _audioService;
    GameRunModel _gameRunModel;
    StatesMachine _gameplayStateMachine;
    GameRunPresenter _progressPresenter;

#endregion

#region Public methods
    
    [Inject]
    public void Construct(GameRunModel gameRunModel, IAudioService audioService)
    {
        _gameRunModel = gameRunModel;
        _audioService = audioService;
    }

    public async UniTask Init(GameObject canvasInputHandler)
    {
        _uiController.Init();
        _playerFacadeGameplay.Init(canvasInputHandler);
        _progressPresenter = new GameRunPresenter(_gameplayView, _gameRunModel);
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
        // Sets the game state by transitioning between states. It could be separate state for each state, but I wanted to show payload state in work
        switch (state)
        {
            case EGameplayState.Play:
                _gameplayStateMachine.Enter<SubStateGameplayPlay, bool>(false).GetAwaiter();
                break;
            case EGameplayState.NewGame:
                _gameplayStateMachine.Enter<SubStateGameplayPlay, bool>(true).GetAwaiter();
                break;
            case EGameplayState.Pause:
                _gameplayStateMachine.Enter<SubStateGameplayPause, bool>(false).GetAwaiter();
                break;
            case EGameplayState.Loose:
                _gameplayStateMachine.Enter<SubStateGameplayPause, bool>(true).GetAwaiter();
                break;
            case EGameplayState.Leave:
                _gameplayEntryPoint.GoToMenu().GetAwaiter();
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
            new SubStateGameplayPause(_uiController, _enemiesSpawner, _playerFacadeGameplay, _progressPresenter, _audioService),
            new SubStateGameplayPlay(_uiController, _enemiesSpawner, _playerFacadeGameplay, _progressPresenter),
        };

        _gameplayStateMachine = new StatesMachine(states);
    }

#endregion
}
}