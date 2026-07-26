using UnityEditor;
using Helpers.Editor;

namespace Source.Core.Editor
{
public abstract class SaveSystemCommandsAdditional : SaveSystemCommands
{
    [MenuItem("Tools/Helpers/SaveAll", false, 2)]
    public static void Save()
    {
        // SessionManager.Current.Save(ESaveFileType.All);
    }
}
}