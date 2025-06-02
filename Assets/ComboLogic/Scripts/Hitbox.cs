 using System;
 using UnityEngine;
 using UnityEngine.Events;

 public class Hitbox : MonoBehaviour, IDamageSender<DamageMessage>
{

 [SerializeField] DamageMessage _damageMessage;


 private void OnTriggerEnter(Collider other)
 {
  if (other.TryGetComponent(out IDamageReciever<DamageMessage> receiver))
  {
   print(other);
   SendDamage(receiver);
  }
 }

 public void SendDamage(IDamageReciever<DamageMessage> receiver)
 {
 
  receiver.RecieveDamage(_damageMessage);
 }
}
