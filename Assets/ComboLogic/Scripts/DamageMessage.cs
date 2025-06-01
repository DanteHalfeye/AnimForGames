using UnityEngine;
using System;

[Serializable]
public struct DamageMessage
{
    public enum DamageLevel
    {
        Small,
        Medium,
        Large
    }
    public GameObject sender;
    public float amount;
    public DamageLevel damageLevel;
}
