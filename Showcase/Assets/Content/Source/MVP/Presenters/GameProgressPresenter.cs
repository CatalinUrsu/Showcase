using R3;
using IdleNumbers;
using Source.Session;

namespace Source.MVP
{
public class GameProgressPresenter
{
#region Fields

    bool _startNewSession;
    readonly CompositeDisposable _disposable;
    readonly GameProgressModel _gameProgressModel;
    readonly IViewGameplay _viewGameplay;

#endregion

#region Public methods

    public GameProgressPresenter(IViewGameplay viewGameplay, GameProgressModel model)
    {
        _viewGameplay = viewGameplay;
        _gameProgressModel = model;
        _disposable = new CompositeDisposable();
    }

    public void Init()
    {
        _disposable.Clear();
        _gameProgressModel.Progress.Value = 0;
        _gameProgressModel.CollectedCoins.Value = 0;
        _gameProgressModel.PlayerIsAlive = true;

        _gameProgressModel.Progress.Skip(1).Subscribe(OnProgressChange_handler).AddTo(_disposable);
        _gameProgressModel.CollectedCoins.Skip(1).Subscribe(OnChangeCoinsAmount_handler).AddTo(_disposable);
        SessionManager.Current.Progress.Lvl.Skip(1).Subscribe(OnReachNewLvl_handler).AddTo(_disposable);

        _viewGameplay.SetNewGameInfo(SessionManager.Current.Progress.Lvl.Value);
    }

    public void Deinit()
    {
        _disposable.Dispose();
        _gameProgressModel.PlayerIsAlive = false;
    }

#endregion

#region Private methods

    void OnChangeCoinsAmount_handler(IdleNumber coins)
    {
        _viewGameplay.OnChangeCoins_handler(coins);
    }

    void OnProgressChange_handler(float progress)
    {
        if (progress < ConstGameplay.PROGRESS_TARGET)
        {
            if (!_startNewSession)
                _viewGameplay.OnChangeProgress_handler(progress);
        }
        else
            SessionManager.Current.Progress.Lvl.Value++;
    }

    void OnReachNewLvl_handler(int lvl)
    {
        SessionManager.Current.Save(ESaveFileType.Progress);
        _startNewSession = true;
        _gameProgressModel.Progress.Value = 0;
        _viewGameplay.PlayNewLvlAnimation(lvl, FinishNewLvlAnimation);
    }
    
    void FinishNewLvlAnimation()
    {
        _startNewSession = false;
        OnProgressChange_handler(_gameProgressModel.Progress.Value);
    }

#endregion
}
}