using UnityEngine;

// [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public abstract class ItemData : ScriptableObject
{
    public string ItemName;
    public Sprite icon;
    public bool isStackable;
}
