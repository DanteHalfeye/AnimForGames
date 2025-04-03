using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class Hunk_Movement : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    [SerializeField] 
    private Transform cameraTransform;

    [SerializeField]
    private VectorDamper_Hunk motionVector = new VectorDamper_Hunk(true);

    private int velXID, velYID;

    public void Awake()
    {
        velXID = Animator.StringToHash("VelX");
        velYID = Animator.StringToHash("VelY");
    }

    public void Move(CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();

        motionVector.TargetValue = direction;

    }

    public void Jump(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            anim.SetTrigger("Jump");
        }
    }

    public void ToggleSprint(CallbackContext ctx)
    {
        bool val = ctx.ReadValueAsButton();
        motionVector.Clamp = !val;
    }

    private void Update()
    {
        motionVector.Update();


        anim.SetFloat(velXID, motionVector.CurrentValue.x);
        anim.SetFloat(velYID, motionVector.CurrentValue.y);
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
