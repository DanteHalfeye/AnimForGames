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
    
    private bool isCharging = false;

    public void OnHeavyAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (Game.Instance.PlayerOne.CurrentStamina > 0)
            {
                // Start charging
                isCharging = true;
                anim.SetTrigger("HeavyAttackCharge"); // start charging animation
            }
        }
        else if (ctx.canceled)
        {
            if (isCharging)
            {
                // Release button => execute charge attack
                isCharging = false;

                anim.SetTrigger("HeavyAttack");  // trigger actual heavy attack animation
                // You can also consume stamina here
            }
        }
    }
    
    public void OnHeavyChargeAnimationEnd()
    {
        if (isCharging)
        {
            isCharging = false;
            anim.ResetTrigger("HeavyAttackCharge");
        }
    }
    
    public void DepleteStamina(float value)
    {
        if (this.gameObject.CompareTag("Player"))
        {
             Game.Instance.PlayerOne.DepleteStamina(value);
            
        }
    }
    public void DepleteStaminaWithParameter(string value)
    {
        if (this.gameObject.CompareTag("Player"))
        {
        Game.Instance.PlayerOne.DepleteStaminaWithParameter(value);
        }
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
