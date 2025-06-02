using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterMovement2 : MonoBehaviour
{
    public enum MotionMode
    {
        Update,
        Physics
    }
    private Animator animator;
    [SerializeField]private FloatDampener speedX, speedY;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMotionVector(float targetX, float targetY)
    {
        speedX.TargetValue = targetX;
        speedY.TargetValue = targetY;
    }

    private void ApplyMotion()
    {
        speedX.Update();
        speedY.Update();
       
        animator.SetFloat("SpeedX", speedX.TargetValue);
        animator.SetFloat("SpeedY", speedY.TargetValue);
    }

    private void Update()
    {
        if (animator.updateMode != AnimatorUpdateMode.Normal) return;
        ApplyMotion();

    }

    private void FixedUpdate()
    {
        if (animator.updateMode != AnimatorUpdateMode.Fixed) return;
        ApplyMotion();
    }
}
