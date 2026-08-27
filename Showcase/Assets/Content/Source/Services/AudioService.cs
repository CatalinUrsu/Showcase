using FMODUnity;
using Source.Data;
using FMOD.Studio;
using Helpers.Audio;

namespace Source.Services
{
public class AudioService : IAudioService
{
#region Fields

    public EventInstance MusicInstance { get; private set; }
    public EventInstance FlyInstance { get; private set; }
    public EventInstance PauseSnapshot { get; private set; }

    VCA _soundVCA;
    VCA _musicVCA;
    FmodEventsSo _fmodEvents;

#endregion

#region Public methods

    public AudioService(FmodEventsSo fmodEventsSO) => _fmodEvents = fmodEventsSO;

    public void Init()
    {
        PauseSnapshot = ConstFMOD.SNAPSHOT_PAUSE.GetInstance();
        
        InitEventInstances();
        InitVCA();
    }

    public void SetSoundVolume(float volume) => _soundVCA.setVolume(volume);
    public void SetMusicVolume(float volume) => _musicVCA.setVolume(volume);

#endregion

#region Private methods

    void InitEventInstances()
    {
        MusicInstance = _fmodEvents.Music.GetInstance();
        MusicInstance.start();

        FlyInstance = _fmodEvents.Fly.GetInstance();
        FlyInstance.SetParameter(ConstFMOD.FLY_POWER, 0);
    }

    void InitVCA()
    {
        _soundVCA = RuntimeManager.GetVCA(ConstFMOD.VCA_Sound);
        _musicVCA = RuntimeManager.GetVCA(ConstFMOD.VCA_Music);
    }

#endregion
}
}