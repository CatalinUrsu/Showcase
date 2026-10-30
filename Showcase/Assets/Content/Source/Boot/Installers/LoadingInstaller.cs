using Zenject;
using Source.UI;
using UnityEngine;

namespace Source.Boot
{
public class LoadingInstaller : MonoInstaller
{
    [SerializeField] SplashScreen _splashScreen;

    public override void InstallBindings()
    {
        var loadingEntryPoint = Container.Resolve<ILoadingEntryPoint>();
        
        loadingEntryPoint.RegisterSplashScreen(_splashScreen);
    }
}
}