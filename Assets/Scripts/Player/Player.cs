using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    Health health = null;
    Shield shield = null;
    Points points = null;

    public event Action<UpgradeType, bool> onUpgrade;

    private void Awake()
    {
        InitializeHealth();
        InitializeShield();
        points = GetComponent<Points>();
    }

    private void InitializeHealth()
    {
        health = GetComponent<Health>();
        health.onDeath += Die;
    }
    private void InitializeShield()
    {
        shield = FindObjectOfType<Shield>();
        shield.onProjectileHit += OnShieldHit;
    }

    private void OnShieldHit(Projectile projectile)
    {
        points.AddPoints(projectile.GetPointsReward());

        projectile.SetIsActive(false);
        projectile.gameObject.SetActive(false);
    }

    private void Die()
    {
        print("Die");
        //Turn off shield movement
        //Stop Projectile launcher
        //Log high score
        //Show end screen(play again/quit)
    }

    public void Upgrade(UpgradeType upgradeType)
    {
        bool isMaxLevel = false;
        int pointCost = int.MaxValue;
   
        if(upgradeType == UpgradeType.Health)
        {
            health.Upgrade();
            isMaxLevel = health.IsMaxLevel();
            pointCost = health.GetNextLevelCost();
        }
        else if (upgradeType == UpgradeType.PointsMultiplier)
        {
            points.UpgradeMultiplier();
            isMaxLevel = points.IsMaxLevel();
            pointCost = points.GetNextLevelCost();
        }
        else if (upgradeType == UpgradeType.Shield)
        {
            shield.Upgrade();
            isMaxLevel = shield.IsMaxLevel();
            pointCost = shield.GetNextLevelCost();
        }

        points.SubtractPoints(pointCost);

        onUpgrade(upgradeType, isMaxLevel);
    }

    public bool CanAffordUpgrade(UpgradeType upgradeType)
    {
        int currentPoints = points.GetPlayerPoints();
        int pointsForNextUpgrade = int.MaxValue;

        if (upgradeType == UpgradeType.Health)
        {
            pointsForNextUpgrade = health.GetNextLevelCost();
        }
        else if (upgradeType == UpgradeType.PointsMultiplier)
        {
            pointsForNextUpgrade = points.GetNextLevelCost();
        }
        else if (upgradeType == UpgradeType.Shield)
        {
            pointsForNextUpgrade = shield.GetNextLevelCost();
        }

        bool cantAfford = currentPoints < pointsForNextUpgrade;

        return !cantAfford;
    }

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile == null) return;

        CastleProjectileTriggerBehavior(projectile);

        projectile.SetIsActive(false);
        projectile.gameObject.SetActive(false);
    }

    private void CastleProjectileTriggerBehavior(Projectile projectile)
    {
        if (projectile.IsArrow()) 
        {
            health.DamageHealth(projectile.GetDamageAmount());
        }
        else if(projectile.GetProjectileType() == ProjectileType.Health)
        {
            health.RestoreHealth(projectile.GetDamageAmount());
        }
        else if(projectile.GetProjectileType() == ProjectileType.Wealth)
        {
            points.AddPoints(projectile.GetPointsReward());
        }
    }

    public Health GetHealth()
    {
        return health;
    }

    public Points GetPoints()
    {
        return points;
    }

    public Shield GetShield()
    {
        return shield;
    }
}
