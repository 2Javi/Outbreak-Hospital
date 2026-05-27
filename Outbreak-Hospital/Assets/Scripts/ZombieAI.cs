using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using NUnit.Framework;

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
        if (isDead) return;

        float Distance = Vector3.Distance(transform.position, Player.position);

        if (Distance <= detectionRange)
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

}
