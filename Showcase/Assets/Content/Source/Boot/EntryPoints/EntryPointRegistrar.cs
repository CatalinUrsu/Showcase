using System;
using Helpers.Services;

namespace Source.Boot
{
/// <summary>
/// Keeps scene entry point in <see cref="EntryPointRegistry"/> while scene container lives.
/// </summary>
public class EntryPointRegistrar<T> : IDisposable where T : class, IEntryPoint
{
    readonly EntryPointRegistry _registry;
    readonly T _entryPoint;

    public EntryPointRegistrar(EntryPointRegistry registry, T entryPoint)
    {
        _registry = registry;
        _entryPoint = entryPoint;
        _registry.Register(_entryPoint);
    }

    public void Dispose() => _registry.Unregister(_entryPoint);
}
}
