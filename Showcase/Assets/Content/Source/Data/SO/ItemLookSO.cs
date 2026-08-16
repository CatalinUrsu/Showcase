using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/Items/ItemAppearance", fileName = "Appearance_", order = 2)]
public class ItemLookSO : ScriptableObject
{
    public ItemIdSo IdSo;
    public Sprite ItemSprite;
}
}