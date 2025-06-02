using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterMovement))]
public class NavMotionController : MonoBehaviour
{
    NavMeshAgent agent;
    CharacterMovement characterMovement;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        characterMovement = GetComponent<CharacterMovement>();
        agent.updateRotation = false;  // We will handle rotation ourselves
        agent.updatePosition = false;  // We will move manually via CharacterMovement
    }

    private void SolveMotion()
    {
        Vector3 navigationDelta = agent.nextPosition - transform.position;

        // Calculate local space movement deltas (right = x, forward = y)
        float deltaX = Vector3.Dot(transform.right, navigationDelta);
        float deltaY = Vector3.Dot(transform.forward, navigationDelta);

        // If close to destination, zero out movement
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            deltaX = 0;
            deltaY = 0;
        }

        // Set target values for smooth animation blend in CharacterMovement
        characterMovement.speedX.TargetValue = deltaX * 1.5f; // tweak multiplier if needed
        characterMovement.speedY.TargetValue = deltaY * 1.5f;
    }

    private void Update()
    {
        SolveMotion();
    }
}