using Helpers;
using Source.Data;

namespace Source.Services
{
public class SessionService : ISessionService
{
    readonly ItemsModel _items;
    readonly ProgressModel _progress;
    readonly SettingsModel _settings;

    public SessionService(ItemsModel items, ProgressModel progress, SettingsModel settings)
    {
        _items = items;
        _progress = progress;
        _settings = settings;
    }

    public void Save(ESaveFileType saveFileType)
    {
        switch (saveFileType)
        {
            case ESaveFileType.All:
                SaveSystem.Save(ConstSavesPaths.ITEMS_PATH, _items.ToSaveData());
                SaveSystem.Save(ConstSavesPaths.PROGRESS_PATH, _progress.ToSaveData());
                SaveSystem.Save(ConstSavesPaths.SETTINGS_PATH, _settings.ToSaveData());
                break;
            case ESaveFileType.Items:
                SaveSystem.Save(ConstSavesPaths.ITEMS_PATH, _items.ToSaveData());
                break;
            case ESaveFileType.Progress:
                SaveSystem.Save(ConstSavesPaths.PROGRESS_PATH, _progress.ToSaveData());
                break;
            case ESaveFileType.Settings:
                SaveSystem.Save(ConstSavesPaths.SETTINGS_PATH, _settings.ToSaveData());
                break;
        }
    }
}
}
