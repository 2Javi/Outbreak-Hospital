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

    NavMeshAgent Agent;
    Animator anim;
    bool isDead = false;
    bool isAttacking;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (isDead) return;

        float Distance = Vector3.Distance(transform.position, Player.position);

        if (Distance <= detectionRange)
        {
            Agent.SetDestination(Player.position);
            // anim.SetBool("isWalking", true);

            if (Distance <= attackDistance && isAttacking)
            {
                // StartCoroutine(PlayAttackAnimation());
            }
        }
        else
        {
            Agent.ResetPath();
            // anim.SetBool("isWalking", false);
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

}
