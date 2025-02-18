using System;
using UnityEngine;

[Serializable]
public struct FloatDampener
{
    [SerializeField] private float smoothTime;
    public float CurrentValue { get; private set; }
    public float TargetValue { get; set; }
    private float currentVelocity;

    public void Update()
    {
        CurrentValue = Mathf.SmoothDamp(CurrentValue, TargetValue, ref currentVelocity, smoothTime);
    }
}