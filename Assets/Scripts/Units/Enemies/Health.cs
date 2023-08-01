using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    public float currentHealth;
    public event Action<float> OnHealthPercentageChange;

    private void OnEnable()
    {
        maxHealth += Game.Instance.enemyHealthMultiplier;
        currentHealth = maxHealth;
    }

    public void ModifyHealth(float amount)
    {
        currentHealth += amount;
        float currentHealthPercentage = currentHealth / maxHealth;
        OnHealthPercentageChange?.Invoke(currentHealthPercentage);
    }
}
