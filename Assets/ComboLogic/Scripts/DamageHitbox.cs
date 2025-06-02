using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;

public class DamageHitbox : MonoBehaviour, IDamageReciever<DamageMessage>
{
    [Serializable]
    public class AttackQueueEvent : UnityEvent<DamageMessage>
    {
  
    }
    
    public AttackQueueEvent OnHit;
    public void RecieveDamage(DamageMessage damage)
    {
        if (damage.sender.transform.root.gameObject == transform.root.gameObject)
        {
            return;
        }
        OnHit?.Invoke(damage);
        print($"recieved damage {damage.amount}");

    }
}
