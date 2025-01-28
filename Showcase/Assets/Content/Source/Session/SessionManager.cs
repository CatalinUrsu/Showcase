using System;
using Helpers;
using Constants;
using Source.MVP;
using Source.Datas;
using System.Collections.Generic;

namespace Source.Session
{
[Serializable]
public class SessionManager
{
#region Fields
    
    public SessionProgress Progress;
    public SessionItems Items;
    public SessionSettings Settings;
    public static SessionManager Current => _currentSessionManager ??= new SessionManager();

    static SessionManager _currentSessionManager;

#endregion

#region Public methods

    public SessionManager()
    {
        Items = SaveSystem.Load<SessionItems>(ConstSavesPaths.ITEMS_PATH) ?? new SessionItems();
        Progress = SaveSystem.Load<SessionProgress>(ConstSavesPaths.PROGRESS_PATH) ?? new SessionProgress();
        Settings = SaveSystem.Load<SessionSettings>(ConstSavesPaths.SETTINGS_PATH)?? new SessionSettings();
    }

    public void Save(ESaveFileType saveFileType)
    {
        switch (saveFileType)
        {
            case ESaveFileType.All:
                SaveSystem.Save(ConstSavesPaths.ITEMS_PATH, Items);
                SaveSystem.Save(ConstSavesPaths.PROGRESS_PATH, Progress);
                SaveSystem.Save(ConstSavesPaths.SETTINGS_PATH, Settings);
                break;
            case ESaveFileType.Items:
                SaveSystem.Save(ConstSavesPaths.ITEMS_PATH, Items);
                break;
            case ESaveFileType.Progress:
                SaveSystem.Save(ConstSavesPaths.PROGRESS_PATH, Progress);
                break;
            case ESaveFileType.Settings:
                SaveSystem.Save(ConstSavesPaths.SETTINGS_PATH, Settings);
                break;
        }
    }

    /// <summary>
    /// Load previous saved SessionPartItems (or create new if there's no saves). Go throught saved items to see if there are some changes
    /// </summary>
    /// <param name="initWeaponsInfo">list of init datas for weapons used to check saved weapon items</param>
    /// <param name="initShipsInfo">list of init datas for ships used to check saved ships items</param>
    public void SetInitDataForItems(Dictionary<string, ItemInitDataWeapon> initWeaponsInfo, Dictionary<string, ItemInitDataShip> initShipsInfo)
    {
        SetWeaponsInitData();
        SetShipsInitData();
        Save(ESaveFileType.Items);

        void SetShipsInitData()
        {
            var cachedItems = new Dictionary<string, ItemModelShip>();

            foreach (var initItem in initShipsInfo)
            {
                if (Items.Ships.ContainsKey(initItem.Key))
                {
                    Items.Ships[initItem.Key].SetInitData(initItem.Value);
                    cachedItems.Add(initItem.Key, Items.Ships[initItem.Key]);
                }
                else
                    cachedItems.Add(initItem.Key, new ItemModelShip(initItem.Value));
            }

            Items.Ships = cachedItems;
        }
        void SetWeaponsInitData()
        {
            var cachedItems = new Dictionary<string, ItemModelWeapon>();

            foreach (var initItem in initWeaponsInfo)
            {
                if (Items.Weapons.ContainsKey(initItem.Key))
                {
                    Items.Weapons[initItem.Key].SetInitData(initItem.Value);
                    cachedItems.Add(initItem.Key, Items.Weapons[initItem.Key]);
                }
                else
                    cachedItems.Add(initItem.Key, new ItemModelWeapon(initItem.Value));
            }

            Items.Weapons = cachedItems;
        }
    }

#endregion
}

public static class ConstSavesPaths
{
    public static readonly string SETTINGS_PATH = ConstSession.SAVES_FOLDER_PATH + "/settings.bin";
    public static readonly string PROGRESS_PATH = ConstSession.SAVES_FOLDER_PATH + "/progress.bin";
    public static readonly string ITEMS_PATH = ConstSession.SAVES_FOLDER_PATH + "/items.bin";
}
}