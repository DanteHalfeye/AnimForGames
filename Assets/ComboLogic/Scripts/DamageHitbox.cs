using UnityEngine;

public class DamageHitbox : MonoBehaviour, IDamageReciever<DamageMessage>
{
    public void RecieveDamage(DamageMessage damage)
    {
        if (damage.sender == transform.root.gameObject)
        {
            return;
        }
        Game.Instance.PlayerOne.DepleteHealth(damage.amount);
    }
}
