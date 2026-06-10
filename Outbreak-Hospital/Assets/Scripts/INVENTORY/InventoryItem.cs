using UnityEngine;

[System.Serializable]public class InventoryItem
{
    public ItemData itemData;
    public float currentDurability;

   public InventoryItem(ItemData data)
    {
        itemData = data;
        if (data is MaskData)
        {
            currentDurability = ((MaskData)data).maxDurability;
        } else
        {
            currentDurability = 0;
        }
    }

    // In InventoryItem.cs
public void DegradeMask(float amount, Inventory inventory)
{
    currentDurability -= amount;
    if (currentDurability <= 0)
    {
        currentDurability = 0;
        inventory.maskSlot = null;
    }
}
}
