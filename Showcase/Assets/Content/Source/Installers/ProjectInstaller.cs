using Zenject;
using Source.Audio;
using Helpers.Services;

namespace Source
{
public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        IProgressTrackingService progressTrackingService = new ProgressTrackingService();
        ISceneLoaderService sceneLoaderService = new SceneLoaderService(progressTrackingService);
        ISplashScreenService splashScreenService = new SplashScreenService(progressTrackingService);
        
        Container.Bind<IAudioService>().FromInstance(new AudioService()).AsSingle();
        Container.Bind<IServiceCamera>().FromInstance(new CameraService()).AsSingle();
        Container.Bind<IProgressTrackingService>().FromInstance(progressTrackingService).AsSingle();
        Container.Bind<ISceneLoaderService>().FromInstance(sceneLoaderService).AsSingle();
        Container.Bind<ISplashScreenService>().FromInstance(splashScreenService).AsSingle();
    }
}
}