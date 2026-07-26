namespace Source
{
public interface ISettingsModelController
{
    ISettingsModel IModel { get; }

    void SetSoundEnabled(bool value);
    void SetMusicEnabled(bool value);

    void SetSoundVolume(float value);
    void SetMusicVolume(float value);

    void SetLocaleIdx(int value);

    void ResetSettings();
}
}