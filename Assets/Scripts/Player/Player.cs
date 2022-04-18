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
        projectileSpawner.Activate();
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
        StartCoroutine(deathScreen.InitiateDeathSequence(points.GetPlayerPoints()));
    }

    public void Upgrade(UpgradeType upgradeType)
    {
        bool isMaxLevel = false;
        float pointCost = 0f;
   
        if(upgradeType == UpgradeType.Health)
        {
            pointCost = health.GetLevelUpCost();
            health.Upgrade();
        }
        else if (upgradeType == UpgradeType.PointsMultiplier)
        {
            isMaxLevel = points.IsMaxLevel();
            if (!isMaxLevel)
            {
                pointCost = points.GetLevelUpCost();
                points.UpgradeMultiplier();
            }
        }
        else if (upgradeType == UpgradeType.Shield)
        {
            isMaxLevel = shield.IsMaxLevel();
            if (!isMaxLevel)
            {
                pointCost = shield.GetNextLevelCost();
                shield.Upgrade();
            }
        }

        points.SubtractPoints(pointCost);

        onUpgrade(upgradeType, isMaxLevel);
    }

    public bool CanAffordUpgrade(float pointsForNextUpgrade)
    {
        float currentPoints = points.GetPlayerPoints();
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
