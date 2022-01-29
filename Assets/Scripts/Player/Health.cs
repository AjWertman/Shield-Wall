using System;
using UnityEngine;

[Serializable]
public class HealthProgression
{
    [SerializeField] int level = 0;
    [SerializeField] int nextLevelCost = 100;

    [SerializeField] float health = 100f;

    public int GetLevel()
    {
        return level;
    }

    public int GetNextLevelCost()
    {
        return nextLevelCost;
    }

    public float GetHealth()
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

    public event Action onDeath;

    private void Awake()
    {
        maxLevel = healthProgressions.Length - 1;
    }

    private void Start()
    {
        castleHealth = healthProgressions[0].GetHealth();
    }

    public void DamageHealth(float damageAmount)
    {
        castleHealth -= damageAmount;
        castleHealth = Mathf.Clamp(castleHealth, 0f, 100f);

        if (castleHealth == 0)
        {
            onDeath();
        }
    }

    public void RestoreHealth(float restoreAmount)
    {
        castleHealth += restoreAmount;
        castleHealth = Mathf.Clamp(castleHealth, 0f, 100f);
    }

    public void Upgrade()
    {
        if (IsMaxLevel()) return;
        currentLevel++;

        HealthProgression currentProgression = GetProgression(currentLevel);
        
        castleHealth = currentProgression.GetHealth();
    }

    public bool IsMaxLevel()
    {
        return currentLevel == maxLevel;
    }

    public int GetNextLevelCost()
    {
        HealthProgression nextProgression = GetProgression(currentLevel);

        return nextProgression.GetNextLevelCost();
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
