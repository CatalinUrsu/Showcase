using System;

namespace Source.Data
{
/// <summary>
/// Whole save file (save.json). Format version is stored by SaveSystem next to the data.
/// </summary>
[Serializable]
public class SaveData
{
    public const int VERSION = 1;

    public ProgressSaveData Progress = new();
    public ItemsSaveData Items = new();
    public SettingsSaveData Settings = new();
}
}
