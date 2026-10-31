using System;

namespace Source.Data
{
/// <summary>
/// Save file format of <see cref="SettingsModel"/>. Fields only.
/// </summary>
[Serializable]
public class SettingsSaveData
{
    public bool Sound = true;
    public bool Music = true;
    public float SoundVolume = 1;
    public float MusicVolume = 1;
    public int LocaleIdx;
}
}
