namespace Source
{
public interface IPauseService
{
    void Pause(EVolumeState volumeState, bool freezeTime);
    void Resume();
}
}
