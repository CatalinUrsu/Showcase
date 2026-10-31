using R3;
using System;
using Zenject;
using IdleNumbers;
using Source.Data;
using Cysharp.Threading.Tasks;

namespace Source.UI
{
public class GameRunPresenter : IDisposable
{
#region External Types

    public class Factory : PlaceholderFactory<IGameplayView, GameRunPresenter> { }

#endregion
    
#region Fields

    bool _startNewSession;
    readonly CompositeDisposable _disposable = new();
    readonly GameRunModel _gameRun;
    readonly IGameplayView _viewGameplay;
    readonly ISessionService _sessionService;
    readonly IGameCommands _gameCommands;
    readonly GameBalanceSO _balance;

#endregion

#region Public methods

    public GameRunPresenter(IGameplayView viewGameplay,
                            GameRunModel gameRun,
                            ProgressModel progress,
                            ISessionService sessionService,
                            IGameCommands gameCommands,
                            GameBalanceSO balance)
    {
        _viewGameplay = viewGameplay;
        _gameRun = gameRun;
        _sessionService = sessionService;
        _gameCommands = gameCommands;
        _balance = balance;

        _gameRun.Progress.Skip(1).Subscribe(OnProgressChange_handler).AddTo(_disposable);
        _gameRun.CollectedCoins.Skip(1).Subscribe(OnChangeCoinsAmount_handler).AddTo(_disposable);
        progress.Lvl.Skip(1).Subscribe(OnReachNewLvl_handler).AddTo(_disposable);

        _viewGameplay.SetNewGameInfo(progress.Lvl.CurrentValue);
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
            _gameRun.StartNewLvlAnim();
            await _viewGameplay.PlayNewLvlAnimation(lvl);
            _gameRun.FinishNewLvlAnim();
        }
    }

#endregion
}
}