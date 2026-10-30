using R3;

namespace Source
{
/// <summary>
/// Player intents from UI. Handled by the active game state only, while it is subscribed.
/// </summary>
public interface IGameCommands
{
    Observable<EGameCommand> Stream { get; }
    void Send(EGameCommand command);
}
}
