using UnityEngine;
using UnityEngine.AI;

public class ChaseState : IStateBehaviour<BasicEnemyAiContext>
{
    public void OnEnter(BasicEnemyAiContext context)
    {

    }

    public void OnUpdate(BasicEnemyAiContext context)
    {
        if (context.target == null)
        {
            context.targetDistance = Vector3.Distance(context.agent.transform.position, context.player.transform.position);
            return;
        }

        NavMeshAgent navAgent = context.agent.GetComponent<NavMeshAgent>();
        navAgent.destination = context.target.position;
        context.targetDistance = navAgent.remainingDistance;
    }
}
