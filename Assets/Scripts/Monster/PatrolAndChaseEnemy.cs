using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAndChaseEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolTargets;
    private int currentIndex = -1;

    [Header("Detection Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 90f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseTimeout = 3f;
    private float chaseTimer = 0f;

    private enum State { Patrol, Chase, Attack }
    private State currentState = State.Patrol;

    private NavMeshAgent agent;
    public event Action onArrived;

    [Header("Attack Settings")]
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float attackDuration = 1.2f;
    private bool isAttacking = false;
    private float lastAttackTime = -999f;

    Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (patrolTargets.Length == 0)
        {
            Debug.LogWarning($"순찰 지점 미지정: {gameObject.name}");
            return;
        }

        onArrived += SetNextDestination;
        SetNextDestination();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                PatrolBehavior();
                if (CanSeePlayer()) SwitchToChase();
                break;

            case State.Chase:
                ChaseBehavior();
                break;

            case State.Attack:
                AttackBehavior();
                break;
        }

        if (isAttacking)
        {
            LookAtPlayer();
        }
    }

    private void LateUpdate()
    {
        if (!isAttacking) 
        {
            float speed = agent.velocity.magnitude;

            animator.SetFloat("Speed", speed);
            animator.SetBool("isWalking", speed > 0.01f);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("isWalking", false);
        }
    }

    void PatrolBehavior()
    {
        if (!agent.pathPending &&
            agent.pathStatus == NavMeshPathStatus.PathComplete &&
            agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            onArrived?.Invoke();
        }
    }

    void ChaseBehavior()
    {
        if (player != null && !isAttacking)
            agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance)
        {
            SwitchToAttack();
            return;
        }

        if (CanSeePlayer())
        {
            chaseTimer = 0f; // reset if enemy can still see the player
        }
        else
        {
            chaseTimer += Time.deltaTime;
            if (chaseTimer >= chaseTimeout)
                SwitchToPatrol();
        }
    }

    void AttackBehavior()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > attackDistance)
        {
            SwitchToChase();
            return;
        }
        if (Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    void SetNextDestination()
    {
        if (patrolTargets.Length == 0) return;

        currentIndex = (currentIndex + 1) % patrolTargets.Length;
        agent.SetDestination(patrolTargets[currentIndex].position);
    }

    void SwitchToChase()
    {
        currentState = State.Chase;
        chaseTimer = 0f;
        Debug.Log("Swithed to Chase");
        // TODO: add sound effects if the state was patrol
    }

    void SwitchToPatrol()
    {
        currentState = State.Patrol;
        SetNextDestination();

        // TODO: add sound effects
    }

    void SwitchToAttack()
    {
        currentState = State.Attack;
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 eyePos = transform.position + Vector3.up * 1.5f;
        Vector3 toPlayer = player.position - eyePos;

        float distance = toPlayer.magnitude;
        if (distance > viewDistance) return false;
        
        float angle = Vector3.Angle(transform.forward, toPlayer);
        if (angle > viewAngle * 0.5f) return false;

        // check obstacles
        Ray ray = new Ray(eyePos, toPlayer.normalized);
        // ray origin can be changed later if model is too small
        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance))
        {
            Debug.DrawRay(eyePos, toPlayer.normalized * viewDistance, Color.red);

            if (hit.transform.CompareTag("Player") || hit.transform.root.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                Debug.Log("Hit: " + hit.transform.name);
            }
        }

        return false;
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        agent.isStopped = true;
        //agent.enabled = false;
        // TODO: animator setting
        // animator.SetTrigger("Attack");
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackDelay);

        // TODO: Damage to Player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackDistance)
        {
            Debug.Log($"{gameObject.name} attacked!");
        }
        yield return new WaitForSeconds(attackDuration - attackDelay);

        

        yield return new WaitForSeconds(0.5f);
        //agent.enabled = true;
        agent.isStopped = false;
        isAttacking = false;
        animator.SetBool("isAttacking", false);

        SwitchToChase();
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);
        }
    }
}
