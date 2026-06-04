using UnityEngine;

public class HandStateManager : MonoBehaviour
{
    private Inventory inventory;
    public bool bothHandsEmpty { get; private set; }

    void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {
        CheckHandState();
    }

    void CheckHandState()
    {
        bool leftHandEmpty = inventory.GetSlot(0) == null;
        bool rightHandEmpty = inventory.GetSlot(1) == null;

        bothHandsEmpty = leftHandEmpty && rightHandEmpty;
    }
}