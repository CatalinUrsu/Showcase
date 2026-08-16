using UnityEngine;
using EditorAttributes;

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/Items/ItemAppearance", fileName = "Appearance_", order = 2)]
public class ItemLookSO : ScriptableObject
{
    public ItemIdSo IdSo;
    [AssetPreview(64, 64)] public Sprite ItemSprite;
}
}