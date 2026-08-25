using Zenject;
using UnityEngine;
using Helpers.Audio;
using Source.UI.Gameplay;
using Source.Game.Player;
using Source.Game.Gameplay;

namespace Source.Boot
{
public class GameplayInstaller : MonoInstaller
{
    [SerializeField] BankLoader _bankLoader;
    [SerializeField] PlayerFacade _playerFacade;
    [SerializeField] GameUIFacade _uiFacade;
    [SerializeField] EnemiesSpawner _enemiesSpawner;
    [SerializeField] Collider2D _gameCameraBounds;

    IGameplayContext _gameplayContext;

    public override void InstallBindings()
    {
        _gameplayContext = Container.Resolve<IGameplayContext>();
        _gameplayContext.RegisterBankLoader(_bankLoader);
        _gameplayContext.RegisterPlayerFacade(_playerFacade);
        _gameplayContext.RegisterUIController(_uiFacade);
        _gameplayContext.RegisterEnemiesController(_enemiesSpawner);
        _gameplayContext.RegisterGameCameraBounds(_gameCameraBounds);
    }
}
}