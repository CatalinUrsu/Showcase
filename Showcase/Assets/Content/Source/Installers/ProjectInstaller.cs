using Zenject;
using Source.Audio;
using StateMachine;

namespace Source
{
public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        IServiceLoadingProgress loadingProgressService = new LoadingProgressService();
        IServiceSceneLoader sceneLoaderService = new SceneLoaderService(loadingProgressService);
        IServiceSplashScreen splashScreenService = new SplashScreenService(loadingProgressService);
        
        Container.BindInterfacesTo<CameraService>().AsSingle();
        Container.BindInterfacesTo<AudioService>().AsSingle();
        Container.Bind<IServiceLoadingProgress>().FromInstance(loadingProgressService).AsSingle();
        Container.Bind<IServiceSceneLoader>().FromInstance(sceneLoaderService).AsSingle();
        Container.Bind<IServiceSplashScreen>().FromInstance(splashScreenService).AsSingle();
    }
}
}