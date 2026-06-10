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
    bool leftHandEmpty = IsSlotEmpty(0);
    bool rightHandEmpty = IsSlotEmpty(1);

    bothHandsEmpty = leftHandEmpty && rightHandEmpty;
}

private bool IsSlotEmpty(int index)
{
    return inventory.IsHandSlotEmpty(index);
}

}