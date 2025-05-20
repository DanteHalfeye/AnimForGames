using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Character : MonoBehaviour
{
    Transform lockTarget;
    [SerializeField]bool isAiming;
    
    public bool IsAiming
    {
        get => isAiming;
        set => isAiming = value;
    }
    public Transform LockTarget
    {
        get => lockTarget;
        set => lockTarget = value;
    }
    
    private void RegisterComponents()
    {
        foreach (ICharacterComponent component in GetComponentsInChildren<ICharacterComponent>())
        {
            component.ParentCharacter = this;
        }
    }

    private void Awake()
    {
        RegisterComponents();
        Cursor.lockState = CursorLockMode.Locked;
    }
}
