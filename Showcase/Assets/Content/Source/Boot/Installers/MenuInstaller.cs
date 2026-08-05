using Zenject;
using Source.UI;
using UnityEngine;
using Source.Player;
using Helpers.Audio;

namespace Source.Boot
{
public class MenuInstaller : MonoInstaller
{
    [SerializeField] BankLoader _bankLoader;
    [SerializeField] PlayerFacade _playerFacade;
    [SerializeField] MenuUIFacade _menuUIFacade;
    
    IMenuContext _menuContext;

    public override void InstallBindings()
    {
        _menuContext = Container.Resolve<IMenuContext>();
        
        _menuContext.RegisterBankLoader(_bankLoader);
        _menuContext.RegisterPlayerFacade(_playerFacade);
        _menuContext.RegisterUIMenuFacade(_menuUIFacade);
    }

    void OnDestroy() => _menuContext.Clear();
}
}