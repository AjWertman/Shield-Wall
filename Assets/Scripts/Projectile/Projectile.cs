using UnityEngine;

public enum ProjectileType { lArrow, rArrow, hArrow, Health, Wealth }

public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileType projectileType = ProjectileType.rArrow;
    
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] int pointsReward = 1;
    [SerializeField] float damage = 20f;

    bool isActive = false;

    private void Update()
    {
        if (isActive)
        {
            MoveForward();
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    public void SetIsActive(bool shouldActivate)
    {
        isActive = shouldActivate;
    }

    public ProjectileType GetProjectileType()
    {
        return projectileType;
    }

    public bool IsActive()
    {
        return isActive;
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
