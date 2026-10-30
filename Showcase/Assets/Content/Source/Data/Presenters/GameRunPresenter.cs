using R3;
using System;
using Zenject;
using IdleNumbers;
using Cysharp.Threading.Tasks;

namespace Source.Data
{
public class GameRunPresenter : IDisposable
{
#region External Types

    public class Factory : PlaceholderFactory<IGameplayView, GameRunPresenter> { }

#endregion
    
#region Fields

    bool _startNewSession;
    readonly CompositeDisposable _disposable = new();
    readonly IGameRunModelController _gameRunModelController;
    readonly IGameplayView _viewGameplay;
    readonly ISessionService _sessionService;
    readonly IGameCommands _gameCommands;
    readonly GameBalanceSO _balance;

#endregion

#region Public methods

    public GameRunPresenter(IGameplayView viewGameplay,
                            IGameRunModelController gameRunModelController,
                            IProgressModelController progressModelController,
                            ISessionService sessionService,
                            IGameCommands gameCommands,
                            GameBalanceSO balance)
    {
        _viewGameplay = viewGameplay;
        _gameRunModelController = gameRunModelController;
        _sessionService = sessionService;
        _gameCommands = gameCommands;
        _balance = balance;

        _gameRunModelController.IModel.ProgressRef.Skip(1).Subscribe(OnProgressChange_handler).AddTo(_disposable);
        _gameRunModelController.IModel.CollectedCoinsRef.Skip(1).Subscribe(OnChangeCoinsAmount_handler).AddTo(_disposable);
        progressModelController.IModel.LvlRef.Skip(1).Subscribe(OnReachNewLvl_handler).AddTo(_disposable);

        _viewGameplay.SetNewGameInfo(progressModelController.IModel.LvlRef.CurrentValue);
    }

    public void ClickPause() => _gameCommands.Send(EGameCommand.Pause);

    public void Dispose() => _disposable.Clear();

#endregion

#region Private methods

    void OnProgressChange_handler(float progress) => _viewGameplay.SetProgressSlider(progress / _balance.ProgressTarget);
    
    void OnChangeCoinsAmount_handler(IdleNumber coins) => _viewGameplay.SetCollectedCoins(coins);

    void OnReachNewLvl_handler(int lvl)
    {
        _sessionService.Save(ESaveFileType.Progress);
        PlayNewLvlAnim().Forget();
        return;

        async UniTaskVoid PlayNewLvlAnim()
        {
            _gameRunModelController.StartNewLvlAnim();
            await _viewGameplay.PlayNewLvlAnimation(lvl);
            _gameRunModelController.FinishNewLvlAnim();
        }
    }

#endregion
}
}