using UnityEngine;
using Helpers.Audio;
using Helpers.Services;

namespace Source
{
public interface IGameplayContext: ISceneContext
{
    IBankLoader BankLoader { get; }
    IPlayerFacade PlayerFacade { get; }
    IGameUIFacade UIFacade { get; }
    IEnemiesSpawner EnemiesSpawner { get; }
    Collider2D GameCameraBounds { get; }
    
    void RegisterBankLoader(IBankLoader bankLoader);
    void RegisterPlayerFacade(IPlayerFacade playerFacade);
    void RegisterUIController(IGameUIFacade uiFacade);
    void RegisterEnemiesController(IEnemiesSpawner enemiesSpawner);
    void RegisterGameCameraBounds(Collider2D gameCameraBounds);
}
}