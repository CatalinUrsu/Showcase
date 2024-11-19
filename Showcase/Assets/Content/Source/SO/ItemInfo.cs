using System;
using UnityEngine;

namespace Source.SO.Items
{
public class ItemInfo : ScriptableObject
{
    public string GUID;

    [ContextMenu("ResetID")]
    public void ResetID() => GUID = Guid.NewGuid().ToString();
}
}