using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] ItemData item;
    Inventory inventoryReference;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Contact");
            inventoryReference = other.gameObject.GetComponent<Inventory>();

            inventoryReference.AddItem(item);
            Destroy(gameObject);
        }
    }
}
