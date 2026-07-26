using System;
using UnityEngine;

namespace Source.Data
{
public class ItemIdSo : ScriptableObject
{
    public string _guid => GUID.ToString(); 
    public Guid GUID;

    [ContextMenu("ResetID")]
    public void ResetID() => GUID = Guid.NewGuid();
}
}