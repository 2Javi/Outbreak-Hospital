using UnityEngine;
using System.Collections;

public class DepositBody : MonoBehaviour
{


    void Start()
    {
        
    }

    
    void Update()
    {
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;

    ZombieAI zombieAI = other.GetComponent<ZombieAI>();

            if (zombieAI == null) return;

            if (zombieAI.mistLogic != null)
            Destroy(zombieAI.mistLogic.gameObject);

                    Destroy(other.gameObject);


    }
}
