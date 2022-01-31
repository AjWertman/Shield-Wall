using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    Health health = null;
    Shield shield = null;
    Points points = null;

    DeathScreen deathScreen = null;
    ProjectileSpawner projectileSpawner = null;
    SoundFXManager sfxManager = null;
    Tutorial tutorial = null;

    public event Action<UpgradeType, bool> onUpgrade;
    public event Action onRestart;

    private void Awake()
    {
        health = GetComponent<Health>();
        health.onDeath += Die;
        shield = FindObjectOfType<Shield>();
        shield.onProjectileHit += OnShieldHit;
        points = GetComponent<Points>();
        deathScreen = FindObjectOfType<DeathScreen>();
        deathScreen.onRestartButton += StartNewGame;
        projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        sfxManager = FindObjectOfType<SoundFXManager>();
        sfxManager.ActivateSounds(true);
        tutorial = FindObjectOfType<Tutorial>();
        tutorial.onTutorialComplete += StartNewGame;
    }

    public void StartNewGame()
    {      
        projectileSpawner.Restart();
        health.Restart();
        shield.Restart();
        points.Restart();
        sfxManager.ActivateSounds(true);
        onRestart();
    }

    private void OnShieldHit(Projectile projectile)
    {
        if (projectile.IsArrow())
        {
            points.AddPoints(projectile.GetPointsReward());
        }

        sfxManager.PlayAudioClip(projectile.GetShieldHitSound());

        projectile.SetIsActive(false);
        projectile.gameObject.SetActive(false);
    }

    private void Die()
    {
        shield.SetCanMove(false);
        projectileSpawner.DeathBehavior();
        sfxManager.ActivateSounds(false);

        StartCoroutine(deathScreen.InitiateDeathSequence(points.GetPlayerPoints()));
    }

    public void Upgrade(UpgradeType upgradeType)
    {
        bool isMaxLevel = false;
        float pointCost = 0f;
   
        if(upgradeType == UpgradeType.Health)
        {
            pointCost = health.GetNextLevelCost();
            health.Upgrade();
            isMaxLevel = health.IsMaxLevel();

        }
        else if (upgradeType == UpgradeType.PointsMultiplier)
        {
            pointCost = points.GetNextLevelCost();
            points.UpgradeMultiplier();
            isMaxLevel = points.IsMaxLevel();
            
        }
        else if (upgradeType == UpgradeType.Shield)
        {
            pointCost = shield.GetNextLevelCost();
            shield.Upgrade();
            isMaxLevel = shield.IsMaxLevel();         
        }

        points.SubtractPoints(pointCost);

        onUpgrade(upgradeType, isMaxLevel);
    }

    public bool CanAffordUpgrade(UpgradeType upgradeType)
    {
        float currentPoints = points.GetPlayerPoints();
        float pointsForNextUpgrade = Mathf.Infinity;

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

        bool canAfford = currentPoints > pointsForNextUpgrade;

        return canAfford;
    }

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile == null) return;

        CastleProjectileTriggerBehavior(projectile);
        sfxManager.PlayAudioClip(projectile.GetCastleHitSound());

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
