using Zenject;
using Source.Data;
using UnityEngine;

namespace Source.Boot
{
public class GameplayInstaller : MonoInstaller
{
    
    [SerializeField] GameplayMediator _gameplayMediator;

    public override void InstallBindings()
    {
        var gameplayContext = Container.Resolve<IGameplayContext>();
        
        Container.Bind<GameRunModel>().FromInstance(new GameRunModel()).AsSingle();
        Container.Bind<IGameplayMediator>().FromInstance(_gameplayMediator).AsSingle();
    }
}
}