using System;
using UnityEngine;
using EditorAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/Items/ItemId", fileName = "IdemId_", order = 2)]
public class ItemIdSo : ScriptableObject
{
    [SerializeField, ReadOnly] string _guid;
    public Guid Guid => Guid.TryParse(_guid, out var parsedGuid) ? parsedGuid : Guid.Empty;

    void OnEnable() => EnsureGuidCreated();
    
    void Reset() => ResetId();

    [Button, ContextMenu("ResetID")]
    public void ResetId()
    {
        _guid = Guid.NewGuid().ToString();
        MarkDirtyInEditor();
    }

    void EnsureGuidCreated()
    {
        
        if (Guid.TryParse(_guid, out _))
            return;

        _guid = Guid.NewGuid().ToString();
        MarkDirtyInEditor();
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    void MarkDirtyInEditor()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            EditorUtility.SetDirty(this);
#endif
    }
}
}
