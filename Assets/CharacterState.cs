using System;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    [SerializeField] private float startStamina = 40;
    [SerializeField] private float staminaRegen = 20;

    [SerializeField] private float startHealth = 10000;
    
    [SerializeField] private float currentStamina;
    [SerializeField] private float currentHealth;

    public float CurrentStamina => currentStamina;
    private void RegenStamina(float regenAmount)
    {
        currentStamina = Mathf.Min(currentStamina + regenAmount, startStamina);
    }

    float GetStaminaDepletion()
    {
        //sistema de inventario -1/statfuerza * 1/buff_fuerza
        return 60;
    }

    public void DepleteStaminaWithParameter(string parameter)
    {
        //float motionValue =  GetComponent<Animator>().GetFloat(parameter);
        //DepleteStamina(motionValue);
    }

    private void Start()
    {
        currentStamina = startStamina;
    }

    public void DepleteStamina(float amount)
    {
        currentStamina -= GetStaminaDepletion() * amount;
    }

    public void DepleteHealth(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            //death
            print($"({name}) Dead");
        }
    }
    private void Update()
    {
        RegenStamina(staminaRegen * Time.deltaTime);
    }
}
