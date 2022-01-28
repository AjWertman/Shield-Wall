using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ProjectileSlot
{
    [SerializeField] ProjectileType projectileType = ProjectileType.Arrow;
    [SerializeField] GameObject[] projectilePrefabs = null;

    public ProjectileType GetProjectileType()
    {
        return projectileType;
    }
}

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] float timeBetweenProjectiles = 2f;

    bool canSpawnProjectiles = true;

    float minYClamp = 0f;
    float maxYClamp = 0f;

    private void Awake()
    {
        Shield shield = FindObjectOfType<Shield>();
        minYClamp = shield.GetMinYClamp();
        maxYClamp = shield.GetMaxYClamp();
    }

    private void Update()
    {
        if (canSpawnProjectiles)
        {
            canSpawnProjectiles = false;
            StartCoroutine(SpawnProjectile());
        }
    }

    private IEnumerator SpawnProjectile()
    {
        SetRandomYPosition();

        GameObject projectileInstance = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Projectile projectile = projectileInstance.GetComponent<Projectile>();

        projectile.LaunchProjectile();

        yield return new WaitForSeconds(timeBetweenProjectiles);
        canSpawnProjectiles = true;
    }

    private void SetRandomYPosition()
    {
        float randomYPosition = UnityEngine.Random.Range(minYClamp, maxYClamp);
        Vector3 newPosition = new Vector3(transform.position.x, randomYPosition, transform.position.z);
        transform.position = newPosition;
    }
}
