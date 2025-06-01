using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private List<DamageMessage> damagelist = new List<DamageMessage>();
    #warning TODO: Desmachetear esta vaina
    [SerializeField] Animator animator;
    public void EnqueueDamage(DamageMessage damage)
    {
        if (damagelist.Any(dmg => dmg.sender == damage.sender))return;
        damagelist.Add(damage);
    }

    private void Update()
    {
        Vector3 damageDirection = Vector3.zero;
        int damageLevel = 0;
        bool isDead = false;
        foreach (DamageMessage damage in damagelist)
        {
            Game.Instance.PlayerOne.DepleteHealth(damage.amount, out isDead);
            damageDirection += (damage.sender.transform.position - transform.position).normalized;
            damageLevel = (int)Mathf.Max(damageLevel, (int)damage.damageLevel);
        }
        

        if (damagelist.Count == 0)return;
        damageDirection = Vector3.ProjectOnPlane(damageDirection.normalized, transform.up);
        float damageAngle = Vector3.SignedAngle(damageDirection, transform.forward, transform.up);
        animator.SetFloat("DamageDirection", (damageAngle/180)*0.5f + 0.5f);
        animator.SetInteger("DamageLevel", damageLevel);
        if (isDead) animator.SetTrigger("Die");
        animator.SetTrigger("Damage");
        damagelist.Clear();
    }
}
