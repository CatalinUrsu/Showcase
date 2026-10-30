using R3;
using System;

namespace Source.Services
{
public class GameCommands : IGameCommands, IDisposable
{
    readonly Subject<EGameCommand> _stream = new();

    public Observable<EGameCommand> Stream => _stream;

    public void Send(EGameCommand command) => _stream.OnNext(command);

    public void Dispose() => _stream.Dispose();
}
}
