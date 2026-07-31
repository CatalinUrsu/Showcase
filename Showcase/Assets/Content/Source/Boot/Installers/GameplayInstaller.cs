using Zenject;
using UnityEngine;
using Source.Data;
using Source.Player;
using Helpers.Audio;
using Source.Gameplay;
using Source.UI.Gameplay;

namespace Source.Boot
{
public class GameplayInstaller : MonoInstaller
{
    [SerializeField] BankLoader _bankLoader;
    [SerializeField] PlayerFacade _playerFacade;
    [SerializeField] GamePanelGameplay _gameplayView;
    [SerializeField] GameUIController _uiController;
    [SerializeField] EnemiesSpawner _enemiesSpawner;

    public override void InstallBindings()
    {
        var gameplayContext = Container.Resolve<IGameplayContext>();
        
        gameplayContext.RegisterBankLoader(_bankLoader);
        gameplayContext.RegisterPlayerFacade(_playerFacade);
        
        Container.Bind<GameRunModel>().FromInstance(new GameRunModel()).AsSingle();
        Container.Bind<IGameplayMediator>().FromInstance(_gameplayMediator).AsSingle();
    }
}
}