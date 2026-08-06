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

#endregion

#region Public methods

    public GameRunPresenter(IGameplayView viewGameplay,
                            IGameRunModelController gameRunModelController,
                            IProgressModelController progressModelController)
    {
        _viewGameplay = viewGameplay;
        _gameRunModelController = gameRunModelController;

        _gameRunModelController.IModel.ProgressRef.Skip(1).Subscribe(OnProgressChange_handler).AddTo(_disposable);
        _gameRunModelController.IModel.CollectedCoinsRef.Skip(1).Subscribe(OnChangeCoinsAmount_handler).AddTo(_disposable);
        progressModelController.IModel.LvlRef.Skip(1).Subscribe(OnReachNewLvl_handler).AddTo(_disposable);

        _viewGameplay.SetNewGameInfo(progressModelController.IModel.LvlRef.CurrentValue);
    }

    public void ClickPause() => _gameRunModelController.OnClickPause_raise();

    public void Dispose() => _disposable.Clear();

#endregion

#region Private methods

    void OnProgressChange_handler(float progress) => _viewGameplay.SetProgressSlider(progress);
    
    void OnChangeCoinsAmount_handler(IdleNumber coins) => _viewGameplay.SetCollectedCoins(coins);

    void OnReachNewLvl_handler(int lvl)
    {
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