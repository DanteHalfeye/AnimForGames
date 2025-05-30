using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    private List<DamageMessage> damagelist = new List<DamageMessage>();
    public void EnqueueDamage(DamageMessage damage)
    {
        if (damagelist.Any(dmg => dmg.sender == damage.sender))return;
        damagelist.Add(damage);
    }

    private void Update()
    {
        foreach (DamageMessage damage in damagelist)
        {
            Game.Instance.PlayerOne.DepleteHealth(damage.amount);
        }
    }
}
