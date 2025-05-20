using UnityEngine;

public interface IDamageReciever<TDamage> where TDamage : struct
{
    void RecieveDamage(TDamage damage);
}
