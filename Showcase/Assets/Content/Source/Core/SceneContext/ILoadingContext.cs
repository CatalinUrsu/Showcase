using Helpers.Services;

namespace Source
{
public interface ILoadingContext
{
    ISplashScreen SplashScreen { get; }

    void RegisterSplashScreen(ISplashScreen splashScreen);
}
}