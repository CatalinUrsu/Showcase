using Helpers.Services;

namespace Source
{
public interface ILoadingEntryPoint
{
    ISplashScreen SplashScreen { get; }

    void RegisterSplashScreen(ISplashScreen splashScreen);
}
}