using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class DC_Controller : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField] 
    private Transform cameraTransform;

    [SerializeField]
    private VectorDamper motionVector = new VectorDamper(true);

    private int _xid, _yid;
    private bool _isSprinting;
    private float _directionMultiplier;
    private Vector2 _currentDirection;
    
    private void Awake()
    {
        _xid = Animator.StringToHash("SpeedX");
        _yid = Animator.StringToHash("SpeedY");
        
        if (animator == null)
            Debug.LogError("Animator is not assigned!", this);
        
        if (cameraTransform == null)
            Debug.LogError("Camera Transform is not assigned!", this);
    }
    
    private void Update()
    {
        motionVector.Update();
        _directionMultiplier = (_isSprinting ? 1f : 0.5f);
        motionVector.TargetValue = _currentDirection * _directionMultiplier;
        
        animator.SetFloat(_xid, motionVector.CurrentValue.x);
        animator.SetFloat(_yid, motionVector.CurrentValue.y);
    }

    public void Move(CallbackContext ctx)
    {
        _currentDirection = ctx.ReadValue<Vector2>();
        motionVector.TargetValue = _currentDirection * _directionMultiplier;
    }

    public void Jump(CallbackContext ctx)
    {
        if (ctx.performed && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            animator.SetTrigger("Jump");
        }
    }

    public void ToggleSprint(CallbackContext ctx)
    {
        _isSprinting = ctx.ReadValueAsButton();
        motionVector.Clamp = !_isSprinting;
        _directionMultiplier = (_isSprinting ? 1f : 0.5f);
        motionVector.TargetValue = _currentDirection * _directionMultiplier;
    }

    private void OnAnimatorMove()
    {
        float interpolate = Mathf.Abs(Vector3.Dot(cameraTransform.forward, transform.up));
        Vector3 forward = Vector3.Lerp(cameraTransform.forward, cameraTransform.up, interpolate);
        Vector3 projected = Vector3.ProjectOnPlane(forward, transform.up);
        Quaternion rotation = Quaternion.LookRotation(projected, transform.up);
        
        animator.rootRotation = Quaternion.Slerp(animator.rootRotation, rotation, motionVector.CurrentValue.magnitude);
        animator.ApplyBuiltinRootMotion();
    }
}
