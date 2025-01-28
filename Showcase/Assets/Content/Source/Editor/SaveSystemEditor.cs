using Helpers;
using UnityEditor;
using Source.Session;

namespace Source.Editor
{
public class SaveSystemEditor{

    [MenuItem("Tools/SaveLoad/ResetSaves")]
    public static void ResetSaves() => SaveSystem.ResetSaves();
    
    [MenuItem("Tools/SaveLoad/Save")]
    public static void Save() => SessionManager.Current.Save(ESaveFileType.All);
}
}