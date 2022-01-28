using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileSlot
{
    [SerializeField] ProjectileType projectileType = ProjectileType.Regular;

    [SerializeField] int amountToCreate = 10;
    [SerializeField] GameObject projectilePrefab = null;

    List<Projectile> projectilesList = new List<Projectile>();

    public ProjectileType GetProjectileType()
    {
        return projectileType;
    }

    public int GetAmountToCreate()
    {
        return amountToCreate;
    }

    public GameObject GetProjectilePrefab()
    {
        return projectilePrefab;
    }

    public List<Projectile> GetProjectilesList()
    {
        return projectilesList;
    }
}

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] ProjectileSlot[] projectileSlots = null;
    
    private void Start()
    {
        CreateProjectiles();
    }

    private void CreateProjectiles()
    {
        foreach(ProjectileSlot projectileSlot in projectileSlots)
        {
            for (int i = 0; i < projectileSlot.GetAmountToCreate(); i++)
            {
                GameObject projectileInstance = Instantiate(projectileSlot.GetProjectilePrefab());
                Projectile projectile = projectileInstance.GetComponent<Projectile>();
                projectileSlot.GetProjectilesList().Add(projectile);
                projectile.SetIsActive(false);
                projectile.gameObject.SetActive(false);
            }
        }
    }

    public Projectile GetInactiveProjectile(ProjectileType projectileType)
    {
        List<Projectile> projectileList = GetProjectileList(projectileType);

        foreach (Projectile projectile in projectileList)
        {
            if (!projectile.IsActive())
            {
                return projectile;
            }
        }

        return null;
    }

    private List<Projectile> GetProjectileList(ProjectileType projectileType)
    {
        ProjectileSlot slotToGet = null;

        foreach(ProjectileSlot projectileSlot in projectileSlots)
        {
            if(projectileType == projectileSlot.GetProjectileType())
            {
                slotToGet = projectileSlot;
            }
        }

        return slotToGet.GetProjectilesList();
    }
}
