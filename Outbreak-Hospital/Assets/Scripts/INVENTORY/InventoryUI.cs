using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Dedicated Slots")]
    public Image maskSlotImage;
    public Image gunSlotImage;
    public Image flashlightSlotImage;

    [Header("Bag Slots")]
    public Image[] bagSlotImages = new Image[4];

    [Header("References")]
    public Inventory inventory;
    [SerializeField] private InputActionReference inputAction;
    [SerializeField] private GameObject inventoryPanel;

    public void RefreshUI()
    {
        UpdateSlot(maskSlotImage, inventory.maskSlot);
        UpdateSlot(gunSlotImage, inventory.gunSlot);
        UpdateSlot(flashlightSlotImage, inventory.flashlightSlot);

        for (int i = 0; i < bagSlotImages.Length; i++)
        {
            UpdateSlot(bagSlotImages[i], inventory.GetSlot(i));
        }
    }

    private void UpdateSlot(Image slotImage, InventoryItem item)
{
    if (item != null && item.itemData != null)
    {
        slotImage.sprite = item.itemData.icon;
        slotImage.color = Color.white;
    }
    else
    {
        slotImage.sprite = null;
        // don't touch color — keeps whatever was set in Inspector
    }
}

    private void OnEnable()
    {
        inputAction.action.Enable();
        inputAction.action.performed += OnInventoryToggle;
    }

    private void OnDisable()
    {
        inputAction.action.Disable();
        inputAction.action.performed -= OnInventoryToggle;
    }

    private void OnInventoryToggle(InputAction.CallbackContext ctx)
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }
}