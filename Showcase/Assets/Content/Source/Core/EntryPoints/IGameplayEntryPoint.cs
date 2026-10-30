using UnityEngine;
using Helpers.Services;

namespace Source
{
public interface IGameplayEntryPoint: IEntryPoint
{
    IPlayerFacade PlayerFacade { get; }
    IGameUIFacade UIFacade { get; }
    IEnemiesSpawner EnemiesSpawner { get; }
    Collider2D GameCameraBounds { get; }
}
}
