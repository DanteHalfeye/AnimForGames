using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]

public class BasicEnemyAi : MonoBehaviour, IStateMachine<BasicEnemyAiContext>
{
    
    public BasicEnemyAiContext context { get; set; }
    public IStateBehaviour<BasicEnemyAiContext> currentState { get; set; }
    private IStateBehaviour<BasicEnemyAiContext>[] states;
    private Dictionary<IStateBehaviour<BasicEnemyAiContext>, StateTransition<BasicEnemyAiContext>[]> transitionsMap;
    public bool EvaluateTransitions()
    {
        foreach( StateTransition<BasicEnemyAiContext> transition in transitionsMap[currentState])
        {
            if (transition.OnEvaluate(context)) {
                ((IStateMachine<BasicEnemyAiContext>)this).SwitchState(transition.to);
                return true;
            }
        }
        return false;
    }
    public void UpdateAI()
    {
        currentState.OnUpdate(context);
    }

    private void Awake()
    {
        states = new[]
        {
            new PatrolState(),
            new ChaseState(),
            (IStateBehaviour<BasicEnemyAiContext>)new AttackState()
        };
        transitionsMap =
            new Dictionary<IStateBehaviour<BasicEnemyAiContext>, StateTransition<BasicEnemyAiContext>[]>();
        transitionsMap.Add(states[0], new[]
        {
            new StateTransition<BasicEnemyAiContext>(states[0], states[1], (_) =>  this.context.target != null)  
        });
        transitionsMap.Add(states[1], new[]
        {
            new StateTransition<BasicEnemyAiContext>(states[1], states[2], (_) => this.context.targetDistance < 1)
        });
          transitionsMap.Add(states[2], new[]
       {
                   new StateTransition<BasicEnemyAiContext>(states[1], states[2], (_) => this.context.targetDistance > 10)
               });
       
               currentState = states[0];
    }
    private void Update()
    {
        EvaluateTransitions();
        UpdateAI();
    }
}
