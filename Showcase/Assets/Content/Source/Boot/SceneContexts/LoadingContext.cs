using Helpers.Services;

namespace Source.Boot
{
public class LoadingContext : ILoadingContext
{
    public ISplashScreen SplashScreen { get; private set; }

    public void RegisterSplashScreen(ISplashScreen splashScreen) => SplashScreen = splashScreen;
}
}