using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class ZombieAI : MonoBehaviour
{
    public int health = 100;
    public Transform Player;
    public float detectionRange = 10f;
    public float attackDistance = 3f;
    public float attackInterval = 2f;
    public float speed = 2f;

    NavMeshAgent Agent;
    Animator anim;
    bool isDead = false;
    bool isAttacking;

    [SerializeField] private GameObject mist;
    [SerializeField] private float sightThreshold = 0.5f;
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        Agent.speed = speed;
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);

        if (isDead) return;

        float Distance = Vector3.Distance(transform.position, Player.position);

        if (CanSeePlayer())
        {
            Agent.SetDestination(Player.position);

            if (Distance <= attackDistance && isAttacking)
            {
                StartCoroutine(PlayAttackAnimation());
            }
        }
        else
        {
            Agent.ResetPath();
            anim.SetBool("isWalking", false);
        }

        IEnumerator PlayAttackAnimation()
        {
            isAttacking = false;
            Agent.isStopped = true;
            anim.SetTrigger("Attack");

            yield return new WaitForSeconds(attackInterval);

            Agent.isStopped = false;
            isAttacking = false;
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
        // 1. range check
        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance > detectionRange) return false;

        // 2. angle check
        Vector3 forwards = transform.forward;
        Vector3 playerDirection = (Player.position - transform.position).normalized;
        float alignment = Vector3.Dot(forwards, playerDirection);
        if (alignment < sightThreshold) return false;

        // 3. line of sight
        Vector3 eyePosition = transform.position + Vector3.up * 1.5f;
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
}

