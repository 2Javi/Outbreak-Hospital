using UnityEngine;

public class Inventory : MonoBehaviour
{
    public const int HAND_SLOTS = 2;
    public const int TOTAL_SLOTS = 6;

    [SerializeField] private ItemData[] slots = new ItemData[TOTAL_SLOTS];

    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < TOTAL_SLOTS; i++)
        {
            if (slots[i] == null)
            {
                slots[i] = item;
                return true;
            }
        }

        Debug.Log("Inventory full.");
        return false;
    }

    public bool HasItem(ItemData item)
    {
        for (int i = 0; i < TOTAL_SLOTS; i++)
        {
            if (slots[i] == item) return true;
        }
        return false;
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < TOTAL_SLOTS; i++)
        {
            if (slots[i] == item)
            {
                slots[i] = null;
                return;
            }
        }
    }

    // Hand slot checks — used by HandStateManager
    public bool IsHandSlotEmpty(int index)
    {
        if (index > 1) return false; // only 0 and 1 are hand slots
        return slots[index] == null;
    }

    // Direct slot access — used by HandStateManager
    public ItemData GetSlot(int index)
    {
        return slots[index];
    }
}