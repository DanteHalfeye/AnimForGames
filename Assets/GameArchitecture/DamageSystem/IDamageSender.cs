using UnityEngine;

public interface IDamageSender<TDamage> where TDamage : struct
{
    void SendDamage(IDamageReciever<TDamage> receiver);
}
