using Zenject;
using Source.UI;
using UnityEngine;
using Helpers.Audio;
using Source.Game.Player;

namespace Source.Boot
{
public class MenuInstaller : MonoInstaller
{
    [SerializeField] BankLoader _bankLoader;
    [SerializeField] PlayerFacade _playerFacade;
    [SerializeField] MenuUIFacade _menuUIFacade;

    public override void InstallBindings()
    {
        var entryPoint = new MenuEntryPoint(_bankLoader, _playerFacade, _menuUIFacade);

        Container.Bind<IMenuEntryPoint>().FromInstance(entryPoint).AsSingle();
        Container.BindInterfacesTo<EntryPointRegistrar<IMenuEntryPoint>>().AsSingle().NonLazy();
    }
}
}
