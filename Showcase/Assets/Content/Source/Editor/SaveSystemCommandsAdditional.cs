using UnityEditor;
using Helpers.Editor;
using Source.Session;

namespace Source.Editor
{
public abstract class SaveSystemCommandsAdditional : SaveSystemCommands
{
    [MenuItem("Tools/Helpers/SaveAll", false, 2)]
    public static void Save() => SessionManager.Current.Save(ESaveFileType.All);
}
}