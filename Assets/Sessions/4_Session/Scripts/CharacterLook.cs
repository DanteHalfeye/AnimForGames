using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterLook : MonoBehaviour, ICharacterComponent
{
    [SerializeField] private Transform target;

    [SerializeField] private FloatDampener horizontalDampener;
    [SerializeField] private FloatDampener verticalDampener;

    //Mouse Sens
    [SerializeField] private float horizontalRotationSpeed;
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] private Vector2 verticalRotationLimits;

    [field: SerializeField] public Character ParentCharacter { get; set; }
    
    float verticalRotation;
    public void OnLook(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        inputValue = inputValue / new Vector2(Screen.width, Screen.height);
        horizontalDampener.TargetValue = inputValue.x;
        verticalDampener.TargetValue = inputValue.y;
    }
    private void ApplyLookRotation()
    {
      
        if (target == null)
        {
            throw new NullReferenceException("Look target is null, asign in the inspector");
        }
        if (ParentCharacter.LockTarget != null)
        {
            Vector3 lookDirection = (ParentCharacter.LockTarget.position - target.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            target.rotation = rotation;
            return;
        }

        target.RotateAround(target.position, Vector3.up, horizontalDampener.CurrentValue * horizontalRotationSpeed *360  * Time.deltaTime);
        verticalRotation += verticalDampener.CurrentValue * verticalRotationSpeed *360 * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalRotationLimits.x, verticalRotationLimits.y);
        Vector3 euler = target.localEulerAngles;
        euler.x = verticalRotation;
        target.localEulerAngles = euler;
    }

    private void Update()
    {
        horizontalDampener.Update();   
        verticalDampener.Update();
        ApplyLookRotation();
    }

   
}
