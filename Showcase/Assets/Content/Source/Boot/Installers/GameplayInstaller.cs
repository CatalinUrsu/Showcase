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

    public override void InstallBindings()
    {
        var entryPoint = new GameplayEntryPoint(_bankLoader, _playerFacade, _uiFacade, _enemiesSpawner, _gameCameraBounds);

        Container.Bind<IGameplayEntryPoint>().FromInstance(entryPoint).AsSingle();
        Container.BindInterfacesTo<EntryPointRegistrar<IGameplayEntryPoint>>().AsSingle().NonLazy();
    }
}
}
