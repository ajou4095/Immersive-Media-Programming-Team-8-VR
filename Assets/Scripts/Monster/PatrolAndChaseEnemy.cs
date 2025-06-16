using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/*
 * This scrtip controls enemy's movement.
 * It patrols, chases and attacks player.
 */

public class PatrolAndChaseEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolTargets;
    private int currentIndex = -1;

    [Header("Detection Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 90f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseTimeout = 3f;
    private float chaseTimer = 0f;

    [HideInInspector] public enum State { Patrol, Chase, Attack }
    [HideInInspector] public State currentState = State.Patrol;

    private NavMeshAgent agent;
    public event Action onArrived;

    [Header("Attack Settings")]
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float attackDelay = 0.5f;
    [SerializeField] private float attackDuration = 1.2f;
    private bool isAttacking = false;
    private float lastAttackTime = -999f;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip attackClip;
    private AudioSource audioSource;

    Animator animator;

    private Enemy enemy;
    private Player player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        enemy = GetComponent<Enemy>();
        player = FindAnyObjectByType<Player>();
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
        // need to check if enemy is dead or not.
        // enemy's animation can be weird without this.
        if (!enemy.isDead)
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
            // change it's destination when arrived.
            onArrived?.Invoke();
        }
    }

    void ChaseBehavior()
    {
        // chase player by updating destination.
        if (playerTransform != null && !isAttacking)
            agent.SetDestination(playerTransform.position);

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // try attack when player is close enough.
        if (distanceToPlayer <= attackDistance)
        {
            SwitchToAttack();
            return;
        }

        // enemy can get to it's original route if it's hard to chase player.
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
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        // stop attacking when player is too far to attack.
        if (distanceToPlayer > attackDistance)
        {
            SwitchToChase();
            return;
        }
        // try attack when it's ready.
        if (Time.time > lastAttackTime + attackCooldown && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    // change it's patrol destination to next.
    void SetNextDestination()
    {
        if (patrolTargets.Length == 0) return;

        currentIndex = (currentIndex + 1) % patrolTargets.Length;
        agent.SetDestination(patrolTargets[currentIndex].position);
    }

    public void SwitchToChase()
    {
        currentState = State.Chase;
        chaseTimer = 0f;
        Debug.Log("Swithed to Chase");
    }

    void SwitchToPatrol()
    {
        currentState = State.Patrol;
        SetNextDestination();
    }

    void SwitchToAttack()
    {
        currentState = State.Attack;
    }

    // check if enemy can see the player or not.
    bool CanSeePlayer()
    {
        if (playerTransform == null) return false;

        // eye position should be located over its collider
        Vector3 eyePos = transform.position + Vector3.up * 1.5f;
        Vector3 toPlayer = playerTransform.position - eyePos;

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

    // matchs enemy's animation and attack timing
    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        agent.isStopped = true;

        animator.SetTrigger("isAttacking");

        yield return new WaitForSeconds(attackDelay);

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= attackDistance)
        {
            Debug.Log($"{gameObject.name} attacked!");
            audioSource.PlayOneShot(attackClip);
            player.TakeDamage(2);
        }
        yield return new WaitForSeconds(attackDuration - attackDelay);

        agent.isStopped = false;
        isAttacking = false;

        yield return new WaitForSeconds(0.5f);
        
        SwitchToChase();
    }

    void LookAtPlayer()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);
        }
    }
}
