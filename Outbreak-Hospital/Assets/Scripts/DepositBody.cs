using UnityEngine;
using System.Collections;

public class DepositBody : MonoBehaviour
{
    ZombieAI zombieAI;

    void Start()
    {
        
    }

    
    void Update()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {

    if (!other.CompareTag("Enemy")) return;

    zombieAI = other.gameObject.GetComponent<ZombieAI>();
    Destroy(zombieAI.mistLogic.gameObject);
    }
}
