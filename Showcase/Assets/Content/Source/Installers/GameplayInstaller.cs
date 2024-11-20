using Zenject;
using Source.MVP;
using UnityEngine;
using Source.Gameplay;

namespace Source
{
public class GameplayInstaller : MonoInstaller
{
    [SerializeField] GameplayMediator _gameplayMediator;

    public override void InstallBindings()
    {
        Container.Bind<GameProgressModel>().FromInstance(new GameProgressModel()).AsSingle();
        Container.Bind<IGameplayMediator>().FromInstance(_gameplayMediator).AsSingle();
    }
}
}