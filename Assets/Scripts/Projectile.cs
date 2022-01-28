using UnityEngine;

public enum ProjectileType { Arrow, Wealth, Health }
public enum ProjectileRarity { Common, Uncommon, Rare }

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileType projectileType = ProjectileType.Arrow;
    [SerializeField] ProjectileRarity projectileRarity = ProjectileRarity.Common;

    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] int pointsReward = 1;
    [SerializeField] float damage = 20f;

    bool canMove = false;

    private void Update()
    {
        if (canMove)
        {
            MoveForward();
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    public void LaunchProjectile()
    {
        canMove = true;
    }

    public ProjectileType GetProjectileType()
    {
        return projectileType;
    }

    public ProjectileRarity GetProjectileRarity()
    {
        return projectileRarity;
    }

    public int GetPointsReward()
    {
        return pointsReward;
    }

    public float GetDamageAmount()
    {
        return damage;
    }
}
