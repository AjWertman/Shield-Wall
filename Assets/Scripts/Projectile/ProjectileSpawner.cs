using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ProjectileSpawnChances
{
    [SerializeField] ProjectileType projectileType = ProjectileType.rArrow;
    [SerializeField] Vector2 chances = Vector2.zero;

    public ProjectileType GetProjectileType()
    {
        return projectileType;
    }

    public Vector2 GetChances()
    {
        return chances;
    }
}

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] ProjectileSpawnChances[] projectileSpawnChances = null;
    [SerializeField] float timeBetweenProjectiles = 2f;

    ProjectilePool projectilePool = null;

    bool canSpawnProjectiles = true;

    float minYClamp = 0f;
    float maxYClamp = 0f;

    private void Awake()
    {
        projectilePool = GetComponent<ProjectilePool>();
        SetLauncherClamps();
    }

    private void Update()
    {
        if (canSpawnProjectiles)
        {
            canSpawnProjectiles = false;
            StartCoroutine(LaunchProjectile());
        }
    }

    private IEnumerator LaunchProjectile()
    {
        SetRandomYPosition();

        ProjectileType randomProjectileType = GetRandomProjectileType();
        Projectile newProjectile = projectilePool.GetProjectile(randomProjectileType);
        newProjectile.gameObject.SetActive(true);

        newProjectile.transform.position = transform.position;
        newProjectile.transform.rotation = transform.rotation;

        newProjectile.SetIsActive(true);

        yield return new WaitForSeconds(timeBetweenProjectiles);
        canSpawnProjectiles = true;
    }

    private void SetRandomYPosition()
    {
        float randomYPosition = UnityEngine.Random.Range(minYClamp, maxYClamp);
        Vector3 newPosition = new Vector3(transform.position.x, randomYPosition, transform.position.z);
        transform.position = newPosition;
    }

    private void SetLauncherClamps()
    {
        Shield shield = FindObjectOfType<Shield>();
        minYClamp = shield.GetMinYClamp();
        maxYClamp = shield.GetMaxYClamp();
    }

    private ProjectileType GetRandomProjectileType()
    {
        int randomArrowInt = UnityEngine.Random.Range(0, 100);

        foreach(ProjectileSpawnChances projectileSpawnChance in projectileSpawnChances)
        {
            Vector2 chances = projectileSpawnChance.GetChances();

            if(chances.x < randomArrowInt && randomArrowInt < chances.y)
            {
                return projectileSpawnChance.GetProjectileType();
            }
        }

        return ProjectileType.rArrow;
    }
}
