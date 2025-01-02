using Helpers.StateMachine;

public readonly struct SplashScreenInfo : ISplashScreenInfo
{
    public bool SkipAnimation { get; }

    public SplashScreenInfo(bool skipAnimation) => SkipAnimation = skipAnimation;
}