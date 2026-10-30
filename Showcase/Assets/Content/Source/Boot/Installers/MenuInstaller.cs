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
        Container.Bind<IBankLoader>().FromInstance(_bankLoader);
        Container.Bind<IPlayerFacade>().FromInstance(_playerFacade);
        Container.Bind<IMenuUIFacade>().FromInstance(_menuUIFacade);

        Container.Bind<IMenuEntryPoint>().To<MenuEntryPoint>().AsSingle();
        Container.BindInterfacesTo<EntryPointRegistrar<IMenuEntryPoint>>().AsSingle().NonLazy();
    }
}
}
