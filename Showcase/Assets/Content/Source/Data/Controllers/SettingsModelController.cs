using UnityEngine;

namespace Source.Data
{
public class SettingsModelController : ISettingsModelController
{
#region Fields

    public ISettingsModel IModel => Model;
    public readonly SettingsModel Model;

#endregion

#region Public methods

    public SettingsModelController(SettingsModel model) => Model = model;

    public void SetSoundEnabled(bool value) => Model.Sound.Value = value;

    public void SetMusicEnabled(bool value) => Model.Music.Value = value;

    public void SetSoundVolume(float value) => Model.SoundVolume.Value = Mathf.Clamp01(value);

    public void SetMusicVolume(float value) => Model.MusicVolume.Value = Mathf.Clamp01(value);

    public void SetLocaleIdx(int value) => Model.LocaleIdx.Value = Mathf.Max(0, value);

    public void ResetSettings()
    {
        Model.Sound.Value = true;
        Model.Music.Value = true;
        Model.SoundVolume.Value = 1f;
        Model.MusicVolume.Value = 1f;
        Model.LocaleIdx.Value = 0;
    }

#endregion
}
}