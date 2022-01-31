using System;
using UnityEngine;

[Serializable]
public class HealthProgression
{
    [SerializeField] int level = 0;
    [SerializeField] float nextLevelCost = 100;

    [SerializeField] float health = 100f;

    public int GetLevel()
    {
        return level;
    }

    public float GetNextLevelCost()
    {
        return nextLevelCost;
    }

    public float GetMaxHealth()
    {
        return health;
    }
}

public class Health : MonoBehaviour
{
    [SerializeField] HealthProgression[] healthProgressions = null;

    [SerializeField] int currentLevel = -1;
    int maxLevel = 0;

    [SerializeField] float castleHealth = 100f;
    [SerializeField] float maxHealth = 100f;

    public event Action onHealthChange;
    public event Action onDeath;

    private void Awake()
    {
        maxLevel = healthProgressions.Length - 1;
        maxHealth = healthProgressions[0].GetMaxHealth();
        castleHealth = maxHealth;
    }

    public void DamageHealth(float damageAmount)
    {
        castleHealth -= damageAmount;
        castleHealth = Mathf.Clamp(castleHealth, 0f, maxHealth);

        onHealthChange();

        if (castleHealth == 0)
        {
            onDeath();
        }
    }

    public void RestoreHealth(float restoreAmount)
    {
        castleHealth += restoreAmount;
        castleHealth = Mathf.Clamp(castleHealth, 0f, maxHealth);

        onHealthChange();
    }

    public void Upgrade()
    {
        if (IsMaxLevel()) return;
        currentLevel++;

        HealthProgression currentProgression = GetProgression(currentLevel);
        
        maxHealth = currentProgression.GetMaxHealth();
        castleHealth = maxHealth;
        onHealthChange();
    }

    public bool IsMaxLevel()
    {
        return currentLevel == maxLevel;
    }

    public float GetNextLevelCost()
    {
        HealthProgression nextProgression = GetProgression(currentLevel);

        return nextProgression.GetNextLevelCost();
    }

    public float GetHealth()
    {
        return castleHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return castleHealth / maxHealth;
    }

    public HealthProgression GetProgression(int level)
    {
        HealthProgression progression = null;

        foreach (HealthProgression healthProgression in healthProgressions)
        {
            if (healthProgression.GetLevel() == level)
            {
                progression = healthProgression;
            }
        }

        return progression;
    }
}
