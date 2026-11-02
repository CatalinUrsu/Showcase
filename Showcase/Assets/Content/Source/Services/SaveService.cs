using R3;
using System;
using Helpers;
using Zenject;
using Source.Data;

namespace Source.Services
{
/// <summary>
/// One save file for all models. Any model change marks save dirty; dirty save is written every 10 s (real time),
/// on app pause/quit (see AppLifecycleListener) and on dispose.
/// </summary>
public class SaveService : ISaveService, IInitializable, IDisposable
{
#region Fields

    static readonly TimeSpan FLUSH_INTERVAL = TimeSpan.FromSeconds(10);

    bool _isDirty;

    readonly ProgressModel _progress;
    readonly ItemsModel _items;
    readonly SettingsModel _settings;
    readonly CompositeDisposable _disposables = new();

#endregion

#region Public methods

    public SaveService(ProgressModel progress, ItemsModel items, SettingsModel settings)
    {
        _progress = progress;
        _items = items;
        _settings = settings;
    }

    public void Initialize()
    {
        Observable.Merge(_progress.Changed, _items.Changed, _settings.Changed)
                  .Subscribe(_ => _isDirty = true)
                  .AddTo(_disposables);

        Observable.Interval(FLUSH_INTERVAL, UnityTimeProvider.UpdateIgnoreTimeScale)
                  .Subscribe(_ => Flush())
                  .AddTo(_disposables);
    }

    public void Flush()
    {
        if (!_isDirty) return;

        var data = new SaveData
        {
            Progress = _progress.ToSaveData(),
            Items = _items.ToSaveData(),
            Settings = _settings.ToSaveData()
        };

        SaveSystem.Save(ConstSavesPaths.SAVE_PATH, data, SaveData.VERSION);
        _isDirty = false;
    }

    public void Dispose()
    {
        _disposables.Dispose();
        Flush();
    }

#endregion
}
}
