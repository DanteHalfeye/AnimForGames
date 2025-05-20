using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class AnimatorActivator : MonoBehaviour
{
    private List<Action> activationEvents = new List<Action>();
    private Animator animator;

    protected virtual void InitializeEvents()
    {
        activationEvents.Add(() =>
        {
            animator.SetBool("CanControl", true);
        });
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        InitializeEvents();
    }

    public void AcitivateAnimator(int eventId)
    {
        activationEvents[eventId]?.Invoke();
    }
}
