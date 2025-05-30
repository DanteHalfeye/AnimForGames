 using System;
 using UnityEngine;
 using UnityEngine.Events;

 public class Hitbox : MonoBehaviour, IDamageSender<DamageMessage>
{

 [SerializeField] float _damage;


 private void OnTriggerEnter(Collider other)
 {
  if (other.TryGetComponent(out IDamageReciever<DamageMessage> receiver))
  {
   SendDamage(receiver);
  }
 }

 public void SendDamage(IDamageReciever<DamageMessage> receiver)
 {
  DamageMessage dmg = new DamageMessage()
  {
   sender = transform.root.gameObject,
   amount = _damage
  };
  receiver.RecieveDamage(dmg);
 }
}
