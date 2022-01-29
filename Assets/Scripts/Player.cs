using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int playerPoints = 0;
    [SerializeField] float castleHealth = 100f;

    Shield shield = null;

    private void Awake()
    {
        shield = FindObjectOfType<Shield>();
        shield.onProjectileHit += OnShieldHit;
    }

    public void AddPoints(int pointsToAdd)
    {
        playerPoints += pointsToAdd;
    }

    public void SubtractPoints(int pointsToSubtract)
    {
        playerPoints -= pointsToSubtract;
        playerPoints = Mathf.Clamp(playerPoints, 0, int.MaxValue);
    }

    private void OnShieldHit(Projectile projectile)
    {
        AddPoints(projectile.GetPointsReward());

        projectile.SetIsActive(false);
        projectile.gameObject.SetActive(false);
    }

    private void DamageHealth(float damageAmount)
    {
        castleHealth -= damageAmount;
        castleHealth = Mathf.Clamp(castleHealth, 0f, 100f);

        if (castleHealth == 0)
        {
            Die();
        }
    }

    private void RestoreHealth(float restoreAmount)
    {
        castleHealth += restoreAmount;
        castleHealth = Mathf.Clamp(castleHealth, 0f, 100f);
    }

    private void Die()
    {
        print("Die");
        //Turn off shield movement
        //Stop Projectile launcher
        //Log high score
        //Show end screen(play again/quit)
    }

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile == null) return;

        ProjectileTriggerBehavior(projectile);

        projectile.SetIsActive(false);
        projectile.gameObject.SetActive(false);
    }

    private void ProjectileTriggerBehavior(Projectile projectile)
    {
        if (IsArrow(projectile)) 
        {
            DamageHealth(projectile.GetDamageAmount());
        }
        else if(projectile.GetProjectileType() == ProjectileType.Health)
        {
            RestoreHealth(projectile.GetDamageAmount());
        }
        else if(projectile.GetProjectileType() == ProjectileType.Wealth)
        {
            AddPoints(projectile.GetPointsReward());
        }
    }

    private bool IsArrow(Projectile projectile)
    {
        ProjectileType type = projectile.GetProjectileType();
        if (type == ProjectileType.lArrow) return true;
        else if (type == ProjectileType.rArrow) return true;
        else if (type == ProjectileType.hArrow) return true;
        else return false;
    }
}
