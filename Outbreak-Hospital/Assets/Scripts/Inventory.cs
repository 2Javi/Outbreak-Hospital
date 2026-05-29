using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxSlots = 4;
    [SerializeField] private List<ItemData> slots;

    private void Start()
    {
        slots = new List<ItemData>();
    }

    public void AddItem(ItemData item)
    {
        if (slots.Count < maxSlots)
        {
            slots.Add(item);
        }
    }

    public bool HasItem(ItemData item)
    {
        return slots.Contains(item);

    }

    public void RemoveItem(ItemData item)
    {
        slots.Remove(item);
    }
}
