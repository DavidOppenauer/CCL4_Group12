using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking
}

public class EnemyBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject player;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected float triggerRange;

    protected NavMeshAgent navMeshAgent;

    protected EnemyState currentState = EnemyState.Idle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                OnIdleState();
                break;
            case EnemyState.Chasing:
                OnChasingState();
                break;
            case EnemyState.Attacking:
                OnAttackingState();
                break;
            default:
                Debug.Log("Something went wrong with the states");
                break;
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("Enemy attacked!");
    }


    #region State Functions

    private void OnIdleState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        navMeshAgent.ResetPath();

        if (distanceToPlayer <= triggerRange)
        {
            currentState = EnemyState.Chasing;
        }

    }

    private void OnChasingState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        navMeshAgent.SetDestination(player.transform.position);

        if (distanceToPlayer <= attackRange && HasLineOfSight())
        {
            currentState = EnemyState.Attacking;
        }
        else if (distanceToPlayer > triggerRange)
        {
            currentState = EnemyState.Idle;
        }

    }

    private void OnAttackingState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Attack();
        navMeshAgent.ResetPath();

        if (distanceToPlayer > attackRange || !HasLineOfSight())
        {
            currentState = EnemyState.Chasing;
        }
    }
    #endregion

    #region Helper Functions
    private bool HasLineOfSight()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Vector3 enemyPos = transform.position;
        Vector3 playerPos = player.transform.position;
        Vector3 direction = (playerPos - enemyPos).normalized;

        if (Physics.Raycast(enemyPos, direction, out RaycastHit hitInfo, distanceToPlayer, ~0))
        {
            if (hitInfo.transform.gameObject == player)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
