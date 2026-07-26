using UnityEngine;

namespace Source.Data
{
[CreateAssetMenu(menuName = "SO/ItemAppearence", fileName = "Appearence_")]
public class ItemLookSO : ScriptableObject
{
    public ItemIdSo IdSo;
    public Sprite ItemSprite;
}
}