using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingScript : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private int gunDamage = 25;

    private void Update()
    {
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * maxDistance, Color.green);
    }

    public void OnShoot(InputValue value)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);

            ZombieAI zombieAI = hit.collider.GetComponent<ZombieAI>();

            if (zombieAI != null)
            {
                zombieAI.TakeDamage(gunDamage);
            }


        }
        else
        {
            Debug.Log("Missed");
        }

    }
}
