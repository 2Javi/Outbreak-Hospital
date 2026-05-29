using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class ZombieAI : MonoBehaviour
{
    public int health = 100;
    public Transform Player;
    public float detectionRange = 10f;
    public float attackDistance = 0.5f;
    public float attackInterval = 4f;
    public float speed = 2f;

    NavMeshAgent Agent;
    Animator anim;
    bool isDead = false;
    public bool isAttacking = true;

    [SerializeField] private GameObject mist;
    [SerializeField] private float sightThreshold = 0.5f;
    MovementStateManager movementStateManagerReference;
    PlayerStats playerStatsReference;
    public float distance = 0f;
    Vector3 playerDirection;
    Vector3 eyePosition;
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        Agent.speed = speed;
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        movementStateManagerReference = Player.gameObject.GetComponent<MovementStateManager>();
        playerStatsReference = Player.gameObject.GetComponent<PlayerStats>();

    }

    private void Update()
    {

        if (anim.runtimeAnimatorController != null)
        {
            anim.SetBool("isWalking", false);
        }

        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);

        if (isDead) return;

        distance = Vector3.Distance(transform.position, Player.position);

        if (CanSeePlayer() || CanHearPlayer())
        {
            Agent.SetDestination(Player.position);

            if (distance <= attackDistance && isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            if (anim.runtimeAnimatorController != null)
            {
                anim.SetBool("isWalking", false);
            }
            Agent.ResetPath();

        }

        IEnumerator AttackPlayer()
        {
            isAttacking = false;
            Agent.isStopped = true;
            // here goes attack animation
            playerStatsReference.health -= 25f;
            yield return new WaitForSeconds(attackInterval);
            Agent.isStopped = false;
            isAttacking = true;

        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log(gameObject.name + " took damage. Health: " + health);

        if (health <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " died");

        Agent.isStopped = true;

        Instantiate(mist, gameObject.transform.position, mist.transform.rotation);
        Destroy(gameObject);
    }

    private bool CanSeePlayer()
    {

        distance = Vector3.Distance(transform.position, Player.position);
        if (distance > detectionRange) return false;

        // 2. angle check
        Vector3 forwards = transform.forward;
        playerDirection = (Player.position - transform.position).normalized;
        float alignment = Vector3.Dot(forwards, playerDirection);
        if (alignment < sightThreshold) return false;

        // 3. line of sight
        eyePosition = transform.position + Vector3.up * 1.5f;
        if (Physics.Raycast(eyePosition, playerDirection, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.gameObject != Player.gameObject) return false;
        }
        else
        {
            return false;
        }

        return true;
    }

    private bool CanHearPlayer()
    {

        distance = Vector3.Distance(transform.position, Player.position);
        playerDirection = (Player.position - transform.position).normalized;
        eyePosition = transform.position + Vector3.up * 1.5f;

        if (movementStateManagerReference.noiseLevel >= distance)
        {
            if (Physics.Raycast(eyePosition, playerDirection, out RaycastHit hit))
            {
                if (hit.collider.gameObject == Player.gameObject)
                {
                    Debug.Log("hit: " + hit.collider.gameObject.name);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

}

