using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterMovement))]
public class EnemyStateMachine : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float attackDistance = 2f;
    public Transform[] patrolPoints;

    private NavMeshAgent agent;
    private CharacterMovement characterMovement;
    private Animator animator;
    private Transform player;
    private bool randomPatrolPoint;
    private bool isCharging;
    Vector3 point;

    private enum State { MoveToPoint, Chase, Attack, Hurt, Die }
    private State currentState;

    private bool isHurt = false;
    private bool isDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        characterMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (patrolPoints.Length > 0)
            randomPatrolPoint = GetRandomPatrolPoint(Vector3.zero, 20, out point);

        currentState = State.MoveToPoint;

        agent.updateRotation = true;
        agent.updatePosition = false;
    }

    private void Update()
    {
        if (isDead)
        {
            currentState = State.Die;
            HandleState();
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackDistance)
            currentState = State.Attack;
        else if (distanceToPlayer <= detectionRadius)
            currentState = State.Chase;
        else
            currentState = State.MoveToPoint;

        HandleState();
        
    }
    public void OnHeavyAttack()
    {
           animator.SetTrigger("HeavyAttackCharge"); // start charging animation
           if (Random.value > 0.5f)
           {
               animator.SetTrigger("HeavyAttack"); // trigger actual heavy attack animation
           }
    }
    
    public void OnHeavyChargeAnimationEnd()
    {
        if (isCharging)
        {
            isCharging = false;
            animator.ResetTrigger("HeavyAttackCharge");
        }
    }
   
    private void HandleState()
    {
        switch (currentState)
        {
            case State.MoveToPoint:
                agent.isStopped = false;
                agent.SetDestination(point);

                MoveCharacterAlongAgent();

                if (Vector3.Distance(transform.position, point) <= 3)
                    GetRandomPatrolPoint(Vector3.zero, 20, out point);

                characterMovement.OnRun(false); // Not running on patrol
                break;

            case State.Chase:
                agent.isStopped = false;
                agent.SetDestination(player.position);

                MoveCharacterAlongAgent();

                characterMovement.OnRun(true); // Running while chasing
                break;

            case State.Attack:
                agent.isStopped = true;

                characterMovement.OnRun(false); // stop running
        
                if (Random.value > 0.5f)
                    OnHeavyAttack();
                else
                    animator.SetTrigger("Attack");

                // Stop movement input while attacking
                characterMovement.OnMove(0f, 0f);
                break;
            
            case State.Die:
                agent.isStopped = true;
                agent.updateRotation = false;
                agent.updatePosition = false;
                characterMovement.OnMove(0f, 0f);
                characterMovement.OnRun(false);
                break;
        }
    }
    private void OnAnimatorMove()
    {
        if (agent == null) return;

        transform.position = animator.rootPosition;
        transform.rotation = animator.rootRotation;

        // Snap agent back to the root motion-driven position
        agent.nextPosition = transform.position;
    }


    private void MoveCharacterAlongAgent()
    {
        Vector3 velocity = agent.desiredVelocity;

// Normalize based on agent speed
        float maxSpeed = agent.speed > 0f ? agent.speed : 1f; // Avoid division by zero

        float deltaX = Vector3.Dot(transform.right, velocity) / maxSpeed;
        float deltaY = Vector3.Dot(transform.forward, velocity) / maxSpeed;

// Optional: clamp to ensure values stay in [-1, 1] range
        deltaX = Mathf.Clamp(deltaX, -1f, 1f);
        deltaY = Mathf.Clamp(deltaY, -1f, 1f);


        characterMovement.speedX.TargetValue = deltaX*1.5f;
        characterMovement.speedY.TargetValue = deltaY*1.5f;
    }


    bool GetRandomPatrolPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void OnEnemyDeath()
    {
        if (!isDead)
        {
            isDead = true;
            currentState = State.Die;
            HandleState();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}