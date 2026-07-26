using Helpers;
using Source.Data;

namespace Source.Services
{
public class SessionService : ISessionService
{
    readonly ItemsModelController _itemsModelController;
    readonly ProgressModelController _progressModelController;
    readonly SettingsModelController _settingsModelController;

    public SessionService(ItemsModelController itemsModelController, ProgressModelController progressModelController, SettingsModelController settingsModelController)
    { 
        _itemsModelController = itemsModelController;
        _progressModelController = progressModelController;
        _settingsModelController = settingsModelController;
    }

    public void Save(ESaveFileType saveFileType)
    {
        switch (saveFileType)
        {
            case ESaveFileType.All:
                SaveSystem.Save(ConstSavesPaths.ITEMS_PATH, _itemsModelController.Model);
                SaveSystem.Save(ConstSavesPaths.PROGRESS_PATH, _progressModelController.Model);
                SaveSystem.Save(ConstSavesPaths.SETTINGS_PATH, _settingsModelController.Model);
                break;
            case ESaveFileType.Items:
                SaveSystem.Save(ConstSavesPaths.ITEMS_PATH, _itemsModelController.Model);
                break;
            case ESaveFileType.Progress:
                SaveSystem.Save(ConstSavesPaths.PROGRESS_PATH, _progressModelController.Model);
                break;
            case ESaveFileType.Settings:
                SaveSystem.Save(ConstSavesPaths.SETTINGS_PATH, _settingsModelController.Model);
                break;
        }
    }
}
}