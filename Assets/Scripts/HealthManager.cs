using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public PlayerController playerScript;
    public Image healthBar;
    public float healthAmount = 100f;

    private void Start()
    {
        healthAmount = playerScript.maxHealth;
    }

    // Update is called once per frame
    void Update() { 
        healthAmount = Mathf.Clamp(playerScript.currentHealth, 0, playerScript.maxHealth);
        healthBar.fillAmount = healthAmount / playerScript.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthAmount = Mathf.Clamp(healthAmount, 0, playerScript.maxHealth);
        healthBar.fillAmount = healthAmount / playerScript.maxHealth;
    }

    public void Heal(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, playerScript.maxHealth);
        healthBar.fillAmount = healthAmount / playerScript.maxHealth;
    }
}
