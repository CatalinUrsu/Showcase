using R3;
using IdleNumbers;

namespace Source.Data
{
public class GameRunModel : IGameRunModel
{
    public ReadOnlyReactiveProperty<float> ProgressRef => Progress;
    public ReadOnlyReactiveProperty<IdleNumber> CollectedCoinsRef => CollectedCoins;
    public ReadOnlyReactiveProperty<bool> PlayerIsKilledRef => PlayerIsKilled;
    public ReadOnlyReactiveProperty<bool> IsPlayingNewLvlAnimRef => IsPlayingNewLvlAnim;

    public ReactiveProperty<float> Progress { get; } = new(0);
    public ReactiveProperty<IdleNumber> CollectedCoins { get; } = new(new IdleNumber());
    public ReactiveProperty<bool> PlayerIsKilled { get; } = new();
    public ReactiveProperty<bool> IsPlayingNewLvlAnim { get; } = new();
}
}