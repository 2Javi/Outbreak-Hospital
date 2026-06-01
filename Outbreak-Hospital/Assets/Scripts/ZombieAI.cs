using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public enum EnemyState
{
    Idle,
    Alert,
    Attacking,
    Dead,
    Resurrected,
    PermanentlyDead
}

public class ZombieAI : MonoBehaviour
{
    // ── Inspector / Tunable Settings ──────────────────────────────
    [Header("Base Settings")]
    public EnemyState currentState;
    public float aggression = 1f;
    public int health = 100;
    public float speed = 2f;

    [Header("Detection Settings")]
    public float detectionRange = 10f;
    [SerializeField] private float sightThreshold = 0.5f;

    [Header("Attack Settings")]
    public float attackDistance = 0.5f;
    public float attackInterval = 4f;

    [Header("Leap Settings")]
    public float leapForce = 10f;
    public float leapRange = 5f;
    public float leapCooldown = 3f;
    public float leapDamage = 20f;

    [Header("References")]
    public Transform Player;
    [SerializeField] private GameObject mist;

    // ── Cached Components ─────────────────────────────────────────
    private Animator animator;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private MovementStateManager movementStateManager;
    private PlayerStats playerStats;

    // ── Animator Hashes ───────────────────────────────────────────
    private int isDeadHash;
    private int isResurrectedHash;
    private int isLeapingHash;
    private int isAlertHash;
    private int isAttackingHash;

    // ── Runtime State ─────────────────────────────────────────────
    private bool isAttacking = true;
    private bool isLeaping = false;
    private float leapCooldownTimer = 0f;
    private float distance = 0f;
    private Vector3 playerDirection;
    private Vector3 eyePosition;
    private void Start()
    {

        movementStateManager = Player.gameObject.GetComponent<MovementStateManager>();
        playerStats = Player.gameObject.GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        isDeadHash = Animator.StringToHash("isDead");
        isResurrectedHash = Animator.StringToHash("isResurrected");
        isLeapingHash = Animator.StringToHash("isLeaping");
        isAlertHash = Animator.StringToHash("isAlert");
        isAttackingHash = Animator.StringToHash("isAttacking");

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = speed;

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

    }

    private void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * 5, Color.red);

        if (currentState == EnemyState.Dead || currentState == EnemyState.PermanentlyDead) return;

        distance = Vector3.Distance(transform.position, Player.position);

        if (CanSeePlayer() || CanHearPlayer())
        {
            agent.SetDestination(Player.position);
            animator.SetBool(isAlertHash, true);

            if (distance <= attackDistance && isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {

            if (agent.isActiveAndEnabled)
            {
                agent.ResetPath();
            }
            animator.SetBool(isAlertHash, false);

        }

        if (currentState != EnemyState.Resurrected) return;

        leapCooldownTimer -= Time.deltaTime;


        if (!isLeaping && leapCooldownTimer <= 0f && distance <= leapRange)
        {
            StartCoroutine(Leap());
        }

        IEnumerator AttackPlayer()
        {
            isAttacking = false;
            agent.isStopped = true;
            animator.SetBool(isAttackingHash, true);
            playerStats.health -= 25f;
            yield return new WaitForSeconds(attackInterval);
            animator.SetBool(isAttackingHash, false);
            agent.isStopped = false;
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

        if (currentState == EnemyState.Resurrected)
        {
            PermanentDeath();
        }
        else
        {
            currentState = EnemyState.Dead;
            animator.SetBool(isDeadHash, true);
            Instantiate(mist, gameObject.transform.position, mist.transform.rotation);
        }
        Debug.Log(gameObject.name + " died");

        agent.isStopped = true;
        Debug.Log("Die() called, currentState: " + currentState);

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

        if (movementStateManager.noiseLevel >= distance)
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

    public void Resurrect()
    {
        currentState = EnemyState.Resurrected;
        aggression = 2f;
        animator.SetBool(isDeadHash, false);
        animator.SetBool(isResurrectedHash, true);
    }

    public void PermanentDeath()
    {
        currentState = EnemyState.PermanentlyDead;
        animator.SetBool(isResurrectedHash, false);
        animator.SetBool(isDeadHash, true);
    }

    private IEnumerator Leap()
    {
        isLeaping = true;
        animator.SetBool(isLeapingHash, true);

        agent.enabled = false;
        rb.isKinematic = false;

        Vector3 direction = (Player.position - transform.position).normalized;
        rb.AddForce(direction * leapForce + Vector3.up * leapForce * 0.5f, ForceMode.Impulse);

        yield return new WaitForSeconds(1.5f); // PLACEHOLDER: tune to match animation length

        rb.isKinematic = true;
        agent.enabled = true;
        animator.SetBool(isLeapingHash, false);

        isLeaping = false;
        leapCooldownTimer = leapCooldown;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isLeaping) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            playerStats.health -= leapDamage;
        }
    }

}

