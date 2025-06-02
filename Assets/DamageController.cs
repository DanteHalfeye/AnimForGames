using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DamageController : MonoBehaviour
{
    private List<DamageMessage> damagelist = new List<DamageMessage>();
    [SerializeField] CharacterMovement.InputType currentInputType = CharacterMovement.InputType.Player;


    [SerializeField] Animator animator;

    public void EnqueueDamage(DamageMessage damage)
    {
        if (damagelist.Any(dmg => dmg.sender == damage.sender)) return;
        damagelist.Add(damage);
    }

    private void Update()
    {
        Vector3 damageDirection = Vector3.zero;
        int damageLevel = 0;
        bool isDead = false;
        foreach (DamageMessage damage in damagelist)
        {
            if (this.currentInputType == CharacterMovement.InputType.Player)
            { Game.Instance.PlayerOne.DepleteHealth(damage.amount, out isDead);
            }
            else
            {
                DepleteHealth(damage.amount, out isDead);
            }

            damageDirection += (damage.sender.transform.position - transform.position).normalized;
            damageLevel = (int)Mathf.Max(damageLevel, (int)damage.damageLevel);
        }


        if (damagelist.Count == 0) return;
        damageDirection = Vector3.ProjectOnPlane(damageDirection.normalized, transform.up);
        float damageAngle = Vector3.SignedAngle(damageDirection, transform.forward, transform.up);
        animator.SetFloat("DamageDirection", (damageAngle / 180) * 0.5f + 0.5f);
        animator.SetInteger("DamageLevel", damageLevel);
        if (isDead) animator.SetTrigger("Die");
        animator.SetTrigger("Damage");
        damagelist.Clear();
    }

    public float EnemyHealth = 1000;

    [System.Serializable]
    public class DeathEvent : UnityEvent {}

    public DeathEvent OnDeath = new DeathEvent();


    public void DepleteHealth(float amount, out bool zeroHealth)
    {
        EnemyHealth -= amount;
        zeroHealth = false;
        if (EnemyHealth <= 0)
        {
            print($"({name}) Dead");
            zeroHealth = true;
            OnDeath.Invoke(); // ✅ UnityEvent
        }
    }

}
