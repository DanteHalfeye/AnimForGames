using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class Carl_Movement : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private VectorDamper_Carl motionVector = new VectorDamper_Carl(true);

    private int velXid, velYid;

    public void Awake()
    {
        anim = GetComponent<Animator>();
        velXid = Animator.StringToHash("SpeedX");
        velYid = Animator.StringToHash("SpeedY");

        anim.enabled = true;
    }

    public void Move(CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();
        if (direction != Vector2.zero) anim.SetBool("isIdle", false);
        else anim.SetBool("isIdle", true);
        motionVector.TargetValue = direction;
    }

    public void Jump(CallbackContext ctx)
    {
        bool val = ctx.performed;
        if (val) anim.SetTrigger("Jump");
    }

    private void Update()
    {
        if (anim == null) return;
        motionVector.Update();
        Vector2 direction = motionVector.CurrentValue;
        anim.SetFloat(velXid, direction.x);
        anim.SetFloat(velYid, direction.y);
    }

    private void OnAnimatorMove()
    {
        float interpolator = Mathf.Abs(Vector3.Dot(cameraTransform.forward, transform.up));
        Vector3 forward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up, interpolator);
        Vector3 projected = Vector3.ProjectOnPlane(forward, transform.up);
        Quaternion rotation = Quaternion.LookRotation(projected, transform.up);
        anim.rootRotation = Quaternion.Slerp(anim.rootRotation, rotation, motionVector.CurrentValue.magnitude);
        anim.ApplyBuiltinRootMotion();
    }
}
