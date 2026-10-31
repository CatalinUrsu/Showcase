using R3;
using IdleNumbers;

namespace Source.Data
{
/// <summary>
/// State of current run. Not saved.
/// </summary>
public class GameRunModel
{
#region Fields

    public ReadOnlyReactiveProperty<float> Progress => _progress;
    public ReadOnlyReactiveProperty<IdleNumber> CollectedCoins => _collectedCoins;
    public ReadOnlyReactiveProperty<bool> PlayerIsKilled => _playerIsKilled;
    public ReadOnlyReactiveProperty<bool> IsPlayingNewLvlAnim => _isPlayingNewLvlAnim;

    public bool CanPause => !_playerIsKilled.Value && !_isPlayingNewLvlAnim.Value;

    readonly ReactiveProperty<float> _progress = new(0);
    readonly ReactiveProperty<IdleNumber> _collectedCoins = new(new IdleNumber());
    readonly ReactiveProperty<bool> _playerIsKilled = new();
    readonly ReactiveProperty<bool> _isPlayingNewLvlAnim = new();

#endregion

#region Public methods

    public void StartRun()
    {
        _progress.Value = 0;
        _collectedCoins.Value = 0;
        _playerIsKilled.Value = false;
    }

    public void KillPlayer() => _playerIsKilled.Value = true;

    public void AddReward(IdleNumber coins, float progressPoints)
    {
        _collectedCoins.Value += coins;
        _progress.Value += progressPoints;
    }

    public void ResetProgress() => _progress.Value = 0;

    public void StartNewLvlAnim() => _isPlayingNewLvlAnim.Value = true;

    public void FinishNewLvlAnim() => _isPlayingNewLvlAnim.Value = false;

#endregion
}
}
