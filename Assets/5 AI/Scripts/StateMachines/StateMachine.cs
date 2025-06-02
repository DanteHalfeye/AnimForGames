using UnityEngine;

public interface IStateMachine<TContext>  where TContext : class
{
    TContext context { get; set; }
    IStateBehaviour<TContext> currentState { get; set; }

    void SwitchState(IStateBehaviour<TContext> nextState)
    {
        currentState = nextState;
        nextState.OnEnter(context) ;
    }

    bool EvaluateTransitions();
}
