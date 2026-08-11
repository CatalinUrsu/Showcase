using Zenject;
using UnityEngine;
using Source.Data;
using Helpers.Audio;
using Source.Game.Gameplay;
using Source.Game.Player;
using Source.UI.Gameplay;

namespace Source.Boot
{
public class GameplayInstaller : MonoInstaller
{
    [SerializeField] BankLoader _bankLoader;
    [SerializeField] PlayerFacade _playerFacade;
    [SerializeField] GameUIFacade _uiFacade;
    [SerializeField] EnemiesSpawner _enemiesSpawner;

    [SerializeField] GamePanelGameplay _gameplayView;

    IGameplayContext _gameplayContext;

    public override void InstallBindings()
    {
        var gameRunModelController = GetGameRunModelController();
        Container.Bind<IGameRunModelController>().FromInstance(gameRunModelController).AsSingle();
        
        _gameplayContext = Container.Resolve<IGameplayContext>();
        _gameplayContext.RegisterBankLoader(_bankLoader);
        _gameplayContext.RegisterPlayerFacade(_playerFacade);
        _gameplayContext.RegisterUIController(_uiFacade);
        _gameplayContext.RegisterEnemiesController(_enemiesSpawner);
        _gameplayContext.RegisterGameRunModelController(gameRunModelController);
    }

    void OnDestroy() => _gameplayContext.Clear();

    GameRunModelController GetGameRunModelController()
    {
        var progressModelController = Container.Resolve<IProgressModelController>();
        var usedShipIdx = progressModelController.IModel.UsedShipIdxRef.CurrentValue;
        var shipModel = Container.Resolve<IItemsModelController>()
                                 .GetShipModel(usedShipIdx);
        var sessionService = Container.Resolve<ISessionService>();
        return new GameRunModelController(shipModel, sessionService, progressModelController);
    }
}
}