using R3;

namespace Source
{
    /// <summary>
    /// Model expose audio toggles, volumes as read-only reactive properties.
    /// </summary>
    /// <remarks>
    /// <b>Obtain instances only from <see cref="ISettingsModelController"/>.</b>
    /// Do not construct, resolve or cache implementations directly. <br/>
    /// </remarks>
    public interface ISettingsModel
    {
    public ReadOnlyReactiveProperty<bool> SoundRef { get; }
    public ReadOnlyReactiveProperty<bool> MusicRef { get; }
    public ReadOnlyReactiveProperty<float> SoundVolumeRef { get; }
    public ReadOnlyReactiveProperty<float> MusicVolumeRef { get; }
    public ReadOnlyReactiveProperty<int> LocaleIdxRef { get; }
}
}