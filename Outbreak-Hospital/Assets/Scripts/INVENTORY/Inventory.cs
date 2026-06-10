using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int TOTAL_SLOTS = 4; // bag slots only now
    
    public InventoryItem maskSlot;
    public InventoryItem gunSlot;
    public InventoryItem flashlightSlot;
    public InventoryUI inventoryUI;
    
    [SerializeField] private InventoryItem[] slots = new InventoryItem[TOTAL_SLOTS];

    public bool AddItem(ItemData item)
{
    if (item is MaskData)
    {
        maskSlot = new InventoryItem(item);
        inventoryUI.RefreshUI();
        return true;
    }
    
    if (item is GunData)
    {
        gunSlot = new InventoryItem(item);
        inventoryUI.RefreshUI();
        return true;
    }
    
    if (item is FlashLightData)
    {
        flashlightSlot = new InventoryItem(item);
        inventoryUI.RefreshUI();
        return true;
    }

    for (int i = 0; i < TOTAL_SLOTS; i++)
    {
        if (slots[i] == null || slots[i].itemData == null)
        {
            slots[i] = new InventoryItem(item);
            inventoryUI.RefreshUI();
            return true;
        }
    }

    Debug.Log("Inventory full.");
    return false;
}

    public bool HasItem(ItemData item)
    {
        if (maskSlot != null && maskSlot.itemData == item) return true;
        if (gunSlot != null && gunSlot.itemData == item) return true;
        if (flashlightSlot != null && flashlightSlot.itemData == item) return true;

        for (int i = 0; i < TOTAL_SLOTS; i++)
        {
            if (slots[i] == null || slots[i].itemData == null) continue;
            if (slots[i].itemData == item) return true;
        }
        return false;
    }

    public void RemoveItem(ItemData item)
{
    if (maskSlot != null && maskSlot.itemData == item) { maskSlot = null; inventoryUI.RefreshUI(); return; }
    if (gunSlot != null && gunSlot.itemData == item) { gunSlot = null; inventoryUI.RefreshUI(); return; }
    if (flashlightSlot != null && flashlightSlot.itemData == item) { flashlightSlot = null; inventoryUI.RefreshUI(); return; }

    for (int i = 0; i < TOTAL_SLOTS; i++)
    {
        if (slots[i] == null || slots[i].itemData == null) continue;
        if (slots[i].itemData == item)
        {
            slots[i].itemData = null;
            inventoryUI.RefreshUI();
            return;
        }
    }
}

    public bool IsHandSlotEmpty(int index)
    {
        if (index == 0) return gunSlot == null || gunSlot.itemData == null;
        if (index == 1) return flashlightSlot == null || flashlightSlot.itemData == null;
        return false;
    }

    public InventoryItem GetSlot(int index)
    {
        return slots[index];
    }
}