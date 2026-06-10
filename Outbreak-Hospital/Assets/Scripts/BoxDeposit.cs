using UnityEngine;

public class BoxDeposit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

        ZombieAI zombie = other.GetComponent<ZombieAI>();

        if (zombie != null && zombie.mistLogic != null)
        {
            Destroy(zombie.mistLogic.gameObject);
        }

        Destroy(other.gameObject);
    }
}