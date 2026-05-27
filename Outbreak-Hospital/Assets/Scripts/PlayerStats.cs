using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float hallucination = 0f;
    private GameObject playerRef;
    public float health = 100f;
    MovementStateManager referenceToMovemementStateManager;
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        referenceToMovemementStateManager = playerRef.GetComponent<MovementStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
            Debug.Log("Player Health is 0");
    }
}
