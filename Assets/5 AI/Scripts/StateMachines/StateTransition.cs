public class StateTransition<TContext> where TContext : class
{
    public delegate bool StateTransitionDelegate(TContext context);

    public IStateBehaviour<TContext> from;
    public IStateBehaviour<TContext> to;

    public StateTransitionDelegate OnEvaluate;

    public StateTransition(IStateBehaviour<TContext> from, IStateBehaviour<TContext> to,StateTransitionDelegate onEvaluate)
    {
        this.from = from;
        this.to = to;
        this.OnEvaluate = onEvaluate;
    }
}
