using R3;
using IdleNumbers;

namespace Source
{
public interface IGameRunModel
{
    public ReadOnlyReactiveProperty<float> ProgressRef { get; }
    public ReadOnlyReactiveProperty<IdleNumber> CollectedCoinsRef { get; }
    public ReadOnlyReactiveProperty<bool> PlayerIsKilledRef { get; }
    public ReadOnlyReactiveProperty<bool> IsPlayingNewLvlAnimRef { get; }
}
}