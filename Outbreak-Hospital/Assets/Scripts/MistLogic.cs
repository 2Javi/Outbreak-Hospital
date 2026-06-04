using UnityEngine;
using System.Collections;

public class MistLogic : MonoBehaviour
{
    private GameObject playerRef;
    PlayerStats referenceToPlayerStats;
    MovementStateManager referenceToMovemementStateManager;
    ZombieAI zombieAI;
    private bool canResurrect = false;
    public ZombieAI owner;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        referenceToPlayerStats = playerRef.GetComponent<PlayerStats>();
        referenceToMovemementStateManager = playerRef.GetComponent<MovementStateManager>();
        Destroy(gameObject, 500);
        StartCoroutine(ResurrectDelay());
    }

    IEnumerator ResurrectDelay()
    {
        yield return new WaitForSeconds(3f); // 
        canResurrect = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("player in mist");
            referenceToPlayerStats.hallucination += 1 * Time.deltaTime;
            referenceToMovemementStateManager.currentSpeed -= 0.1f * Time.deltaTime;
            referenceToPlayerStats.health -= 10f * Time.deltaTime;
        }

        if (canResurrect)
        {
            ZombieAI enemy = other.GetComponent<ZombieAI>();

            if (enemy == null || enemy == owner) return;
            
            if (enemy != null && enemy.currentState == EnemyState.Dead)
            {
                Debug.Log(enemy.currentState);
                enemy.Resurrect();
            }
        }
    }
}
