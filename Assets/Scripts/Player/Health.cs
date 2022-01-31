using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] HealthProgression[] healthProgressions = null;
    [SerializeField] MeshRenderer[] damageRenderers = null;

    [SerializeField] int currentLevel = -1;
    int maxLevel = 0;

    [SerializeField] float castleHealth = 100f;
    [SerializeField] float maxHealth = 100f;

    bool isDead = false;

    public event Action onHealthChange;
    public event Action onDeath;

    private void Awake()
    {
        maxLevel = healthProgressions.Length - 1;
        maxHealth = healthProgressions[0].GetMaxHealth();
        castleHealth = maxHealth;
        SetDamageRenderers(100);
    }

    public void DamageHealth(float damageAmount)
    {
        if (isDead) return;
        castleHealth -= damageAmount;
        castleHealth = Mathf.Clamp(castleHealth, 0f, maxHealth);

        StartCoroutine(DamageBehavior());

        onHealthChange();

        if (castleHealth == 0)
        {
            isDead = true;
            onDeath();
        }
    }

    private IEnumerator DamageBehavior()
    {
        SetDamageRenderers(1);

        yield return new WaitForSeconds(.2f);

        SetDamageRenderers(100);
    }

    private void SetDamageRenderers(float fresnelPower)
    {
        foreach (MeshRenderer meshRenderer in damageRenderers)
        {
            meshRenderer.material.SetFloat("_fresnelPower", fresnelPower);
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

    public void Restart()
    {
        currentLevel = 0;
        maxLevel = healthProgressions.Length - 1;
        maxHealth = healthProgressions[0].GetMaxHealth();
        castleHealth = maxHealth;
        isDead = false;

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