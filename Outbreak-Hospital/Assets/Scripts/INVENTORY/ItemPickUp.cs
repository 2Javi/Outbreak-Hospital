using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] ItemData item;
    private Inventory inventoryReference;
    private bool isInRange = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventoryReference = other.gameObject.GetComponent<Inventory>();
            isInRange = true;
            Debug.Log("Press F to pick up");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            inventoryReference = null;
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            bool added = inventoryReference.AddItem(item);

            if (added)
            {
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory full, cannot pick up.");
            }
        }
    }
}