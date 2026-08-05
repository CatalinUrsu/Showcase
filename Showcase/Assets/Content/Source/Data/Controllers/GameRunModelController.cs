using R3;
using System;
using IdleNumbers;

namespace Source.Data
{
public class GameRunModelController : IGameRunModelController
{
#region Fields

    public IGameRunModel IModel => _model;
    public event Action OnClickPause;
    public event Action OnClickGoHome;
    public event Action OnClickReturnToGame;
    public event Action OnPlayerLoose;

    float _lvlCoinBonus;
    
    readonly float _shipCoinBonus;
    readonly ISessionService _sessionService;
    readonly IProgressModelController _progressModelController;
    readonly GameRunModel _model;
    readonly CompositeDisposable _disposables = new();

#endregion

#region Public methods

    public GameRunModelController(IShipModel shipModel, 
                                  ISessionService sessionService,
                                  IProgressModelController progressModelController)
    {
        _model = new GameRunModel();
        
        _shipCoinBonus = shipModel.EnemyCoinBonusRef.CurrentValue / 100 + 1;
        _sessionService = sessionService;
        _progressModelController = progressModelController;

        SetLvlCoinBonus(progressModelController.IModel.LvlRef.CurrentValue);
        
        _model.ProgressRef.Skip(1).Subscribe(OnProgressChange_handler).AddTo(_disposables);
        progressModelController.IModel.LvlRef.Skip(1).Subscribe(OnReachNewLvl_handler).AddTo(_disposables);
    }

    public void StartRun()
    {
        _model.Progress.Value = 0;
        _model.CollectedCoins.Value = 0;
        _model.PlayerIsKilled.Value = false;
    }

    public void KillPlayer(bool alive)
    {
        _model.PlayerIsKilled.Value = true;
        OnPlayerLoose?.Invoke();
    }

    public void AddCoinsReward(float rewardPoints, IdleNumber coins)
    {
        if (_model.PlayerIsKilledRef.CurrentValue) return;

        var rewardCoins = (_lvlCoinBonus * coins + coins) * _shipCoinBonus;

        _progressModelController.AddCoins(rewardCoins);
        _model.CollectedCoins.Value += rewardCoins;
        _model.Progress.Value += rewardPoints;
    }

    public void OnClickPause_raise()
    {
        if (CanOpenPauseMenu())
            OnClickPause?.Invoke();
    }

    public void OnClickGoHome_raise() => OnClickGoHome?.Invoke();
    public void OnClickReturnToGame_raise() => OnClickReturnToGame?.Invoke();
    
    public void StartNewLvlAnim() => _model.IsPlayingNewLvlAnim.Value = true;
    public void FinishNewLvlAnim() => _model.IsPlayingNewLvlAnim.Value = false;

    public void Dispose() => _disposables.Dispose();

#endregion

#region Private methods

    void OnReachNewLvl_handler(int lvl)
    {
        SetLvlCoinBonus(lvl);
        _model.Progress.Value = 0;
        _sessionService.Save(ESaveFileType.Progress);
    }

    void OnProgressChange_handler(float progress)
    {
        if (progress >= ConstGameplay.PROGRESS_TARGET)
            _progressModelController.AddLevel();
    }


    bool CanOpenPauseMenu() => !_model.PlayerIsKilledRef.CurrentValue && !_model.IsPlayingNewLvlAnimRef.CurrentValue;

    void SetLvlCoinBonus(int lvl) => _lvlCoinBonus = lvl * ConstGameplay.COINS_LVL_BONUS_MULTIPLIER;

#endregion
}
}