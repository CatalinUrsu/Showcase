using R3;
using System;

namespace Source.Session
{
[Serializable]
public class SessionSettings
{
    public ReactiveProperty<bool> Sound { get; private set; }
    public ReactiveProperty<bool> Music { get; private set; }
    public ReactiveProperty<float> SoundVolume { get; private set; }
    public ReactiveProperty<float> MusicVolume { get; private set; }
    public ReactiveProperty<int> LocaleIdx { get; private set; }

    public SessionSettings()
    {
        Sound = new ReactiveProperty<bool>(true);
        Music = new ReactiveProperty<bool>(true);
        SoundVolume = new ReactiveProperty<float>(1);
        MusicVolume = new ReactiveProperty<float>(1);
        LocaleIdx = new ReactiveProperty<int>(0);
    }

    public SessionSettings(SessionSettings deserializeObject)
    {
        Sound = deserializeObject.Sound;
        Music = deserializeObject.Music;
        LocaleIdx = deserializeObject.LocaleIdx;
    }
}
}