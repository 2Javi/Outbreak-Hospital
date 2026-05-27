using UnityEngine;

public class MistLogic : MonoBehaviour
{
    private GameObject playerRef;
    PlayerStats referenceToPlayerStats;
    MovementStateManager referenceToMovemementStateManager;
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        referenceToPlayerStats = playerRef.GetComponent<PlayerStats>();
        referenceToMovemementStateManager = playerRef.GetComponent<MovementStateManager>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player in mist");
            referenceToPlayerStats.hallucination += 1 * Time.deltaTime;
            referenceToMovemementStateManager.moveSpeed -= 0.1f * Time.deltaTime;
            referenceToPlayerStats.health -= 10f * Time.deltaTime;
        }

        // increatese it by 1 
    }
}
