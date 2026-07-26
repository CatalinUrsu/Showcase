using R3;
using System;
using IdleNumbers;

namespace Source.Data
{
public class GameRunPresenter : IDisposable
{
#region Fields

    bool _startNewSession;
    readonly CompositeDisposable _disposable = new();
    readonly IGameRunModelController _gameRunModelController;
    readonly IProgressModelController _progressModelController;
    readonly IViewGameplay _viewGameplay;

#endregion

#region Public methods

    public GameRunPresenter(IViewGameplay viewGameplay, IGameRunModelController gameRunModelController, IProgressModelController progressModelController)
    {
        _viewGameplay = viewGameplay;
        _gameRunModelController = gameRunModelController;
        _progressModelController = progressModelController;

        // Subscribe to model changing
        _gameRunModelController.IModel.ProgressRef.Skip(1).Subscribe(OnProgressChange_handler).AddTo(_disposable);
        _gameRunModelController.IModel.CollectedCoinsRef.Skip(1).Subscribe(OnChangeCoinsAmount_handler).AddTo(_disposable);
        progressModelController.IModel.LvlRef.Skip(1).Subscribe(OnReachNewLvl_handler).AddTo(_disposable);

        _viewGameplay.SetNewGameInfo(progressModelController.IModel.LvlRef.CurrentValue);
    }

    public void Dispose() => _disposable.Clear();

#endregion

#region Private methods

    void OnChangeCoinsAmount_handler(IdleNumber coins) => _viewGameplay.OnChangeCoins_handler(coins);

    void OnProgressChange_handler(float progress) => _viewGameplay.OnChangeProgress_handler(progress);

    void OnReachNewLvl_handler(int lvl)
    {
        _viewGameplay.PlayNewLvlAnimation(lvl, FinishNewLvlAnimation);
    }
    
    void FinishNewLvlAnimation()
    {
        OnProgressChange_handler(_gameRunModelController.Progress.Value);
    }

#endregion
}
}