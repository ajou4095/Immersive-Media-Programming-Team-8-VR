using System;
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

    private enum State { Patrol, Chase }
    private State currentState = State.Patrol;

    private NavMeshAgent agent;
    public event Action onArrived;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
        }

        Debug.DrawRay(transform.position + Vector3.up * 1.5f,
                  (player.position - transform.position).normalized * viewDistance,
                  Color.red);
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
        if (player != null)
            agent.SetDestination(player.position);

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
        // TODO: add sound effects
    }

    void SwitchToPatrol()
    {
        currentState = State.Patrol;
        SetNextDestination();

        // TODO: add sound effects
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        if (distance > viewDistance) return false;
        
        float angle = Vector3.Angle(transform.forward, toPlayer);
        if (angle > viewAngle * 0.5f) return false;

        // check obstacles
        Ray ray = new Ray(transform.position + Vector3.up * 1.5f, toPlayer.normalized);
        // ray origin can be changed later if model is too small
        if (Physics.Raycast(ray, out RaycastHit hit, viewDistance))
        {
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
}
