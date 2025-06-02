using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterState : MonoBehaviour,ICharacterComponent
{
    [SerializeField] private float startStamina = 40;
    [SerializeField] private float staminaRegen = 15;

    [SerializeField] private float startHealth = 50;
    
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
      float motionValue =  ParentCharacter.GetComponentInChildren<Animator>().GetFloat(parameter);
        DepleteStamina(motionValue);
    }

    private void Start()
    {
        currentStamina = startStamina;
        currentHealth = startHealth;
        ParentCharacter = FindFirstObjectByType<Character>();
    }

    public void DepleteStamina(float amount)
    {
        currentStamina -= GetStaminaDepletion() * amount;
    }

    public void DepleteHealth(float amount, out bool zeroHealth)
    {
        currentHealth -= amount;
        zeroHealth = false;
        if (currentHealth <= 0)
        {
            print($"({name}) Dead");
            zeroHealth = true;
            StartCoroutine(RestartSceneAfterDelay(3f));
        }
    }
    private System.Collections.IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void Update()
    {
        RegenStamina(staminaRegen * Time.deltaTime);
    }

    public Character ParentCharacter { get; set; }
}
