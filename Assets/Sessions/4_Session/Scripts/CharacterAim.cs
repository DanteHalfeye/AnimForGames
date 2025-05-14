using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CharacterAim : MonoBehaviour, ICharacterComponent
{
    [SerializeField] CinemachineCamera _aimingCamera;
    [SerializeField] private AimConstraint _aimConstraint;
    [SerializeField] private FloatDampener _aimingDampener;
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnAim(InputAction.CallbackContext ctx)
    {
        if (!ctx.started && !ctx.canceled) return;
        _aimingCamera?.gameObject.SetActive(ctx.started);
        ParentCharacter.IsAiming = ctx.started;
        _aimConstraint.enabled = ctx.started;
        _aimingDampener.TargetValue = ctx.started ? 1 : 0;
        
    }

    private void Update()
    {
        _aimingDampener.Update();
        _aimConstraint.weight = _aimingDampener.CurrentValue;
        _animator.SetLayerWeight(1, _aimingDampener.CurrentValue);
    }

    public Character ParentCharacter { get; set; }
}
