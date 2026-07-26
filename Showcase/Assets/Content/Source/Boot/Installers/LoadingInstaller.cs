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
        var loadingContext = Container.Resolve<ILoadingContext>();
        
        loadingContext.RegisterSplashScreen(_splashScreen);
    }
}
}