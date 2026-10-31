using R3;
using System;
using Zenject;

namespace Source.Data
{
public class SettingsPresenter : IDisposable
{
#region External Types

    public class Factory : PlaceholderFactory<ISettingsView, SettingsPresenter> { }

#endregion

#region Fields

    readonly CompositeDisposable _disposables = new();

#endregion

#region Public methods

    public SettingsPresenter(ISettingsView view, SettingsModel settings, IAudioService audioService, ILocaleService localeService)
    {
        view.SoundVolumeChanged.Subscribe(settings.SetSoundVolume).AddTo(_disposables);
        view.MusicVolumeChanged.Subscribe(settings.SetMusicVolume).AddTo(_disposables);
        view.SoundClicked.Subscribe(_ => settings.ToggleSound()).AddTo(_disposables);
        view.MusicClicked.Subscribe(_ => settings.ToggleMusic()).AddTo(_disposables);
        view.LanguageClicked.Subscribe(_ => settings.SetNextLocale(localeService.LocalesCount)).AddTo(_disposables);
        
        settings.SoundVolume.Subscribe(volume =>
        {
            audioService.SetSoundVolume(volume);
            view.ShowSound(volume > 0, volume);
        }).AddTo(_disposables);

        settings.MusicVolume.Subscribe(volume =>
        {
            audioService.SetMusicVolume(volume);
            view.ShowMusic(volume > 0, volume);
        }).AddTo(_disposables);

        settings.LocaleIdx.Skip(1).Subscribe(localeService.SetLocale).AddTo(_disposables);
    }

    public void Dispose() => _disposables.Dispose();

#endregion
}
}
