using System;
using UnityEngine;
using UnityEngine.InputSystem;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

[RequireComponent(typeof(Animator))]
public class AttackController : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    
    private AttackHitboxController hitboxController;
    
    public void OnLightAttack(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (Game.Instance.PlayerOne.CurrentStamina > 0)
                anim.SetTrigger("Attack");
        }
    }
    
    public void OnHeavyAttack(CallbackContext ctx)
    {
        if (ctx.performed || ctx.canceled)
        {
            if (Game.Instance.PlayerOne.CurrentStamina > 0)
                anim.SetTrigger("HeavyAttack");
        }
    }

    public void DepleteStamina(float value)
    {
        Game.Instance.PlayerOne.DepleteStamina(value);
    }
    public void DepleteStaminaWithParameter(string value)
    {
        Game.Instance.PlayerOne.DepleteStaminaWithParameter(value);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        hitboxController = GetComponent<AttackHitboxController>();
    }

    public void ToggleHitbox(int hitboxId)
    {
        hitboxController.ToggleHitboxes(hitboxId);
    }

    public void CleanupHitboxes()
    {
        hitboxController.CleanupHitboxes();
    }
}
