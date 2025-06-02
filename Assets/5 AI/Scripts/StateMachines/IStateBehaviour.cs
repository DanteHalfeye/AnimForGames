using UnityEngine;

public interface IStateBehaviour<TContext> where TContext  : class
{
  void OnEnter(TContext context);
  void OnUpdate(TContext context);
}
  