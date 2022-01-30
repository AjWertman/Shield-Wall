using UnityEngine;

public enum ProjectileType { lArrow, rArrow, hArrow, Health, Wealth }

public class Projectile : MonoBehaviour
{
    [SerializeField] ProjectileType projectileType = ProjectileType.rArrow;
    
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float pointsReward = 1;
    [SerializeField] float damage = 20f;

    bool isActive = false;

    TrailRenderer trail = null;

    private void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }

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

        if (trail == null) return;
        trail.gameObject.SetActive(shouldActivate);
        trail.Clear();
    }

    public ProjectileType GetProjectileType()
    {
        return projectileType;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public float GetPointsReward()
    {
        return pointsReward;
    }

    public float GetDamageAmount()
    {
        return damage;
    }

    public bool IsArrow()
    {
        if (projectileType == ProjectileType.lArrow) return true;
        else if (projectileType == ProjectileType.rArrow) return true;
        else if (projectileType == ProjectileType.hArrow) return true;
        else return false;
    }
}
