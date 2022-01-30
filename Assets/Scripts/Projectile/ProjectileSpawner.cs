using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ProjectilesSpawnProgression
{
    [SerializeField] int level = 0;
    [SerializeField] float timeBetweenProjectiles = 2f;

    public int GetLevel()
    {
        return level;
    }

    public float GetTimeBetweenProjectiles()
    {
        return timeBetweenProjectiles;
    }
}

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
    [SerializeField] ProjectilesSpawnProgression[] projectilesSpawnProgressions = null;
    [SerializeField] ProjectileSpawnChances[] projectileSpawnChances = null;
    [SerializeField] float timeBetweenProjectiles = 2f;

    ProjectilePool projectilePool = null;

    bool canSpawnProjectiles = true;

    float minYClamp = 0f;
    float maxYClamp = 0f;

    int level = 0;
    int maxLevel = 0;

    private void Awake()
    {
        projectilePool = GetComponent<ProjectilePool>();
        SetLauncherClamps();

        maxLevel = projectilesSpawnProgressions.Length - 1;
        timeBetweenProjectiles = projectilesSpawnProgressions[0].GetTimeBetweenProjectiles();
    }

    private void Update()
    {
        if (canSpawnProjectiles)
        {
            canSpawnProjectiles = false;
            StartCoroutine(LaunchProjectile());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AdvanceLevel();
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

    public void AdvanceLevel()
    {
        if (level + 1 > maxLevel) return;
        level++;

        timeBetweenProjectiles = projectilesSpawnProgressions[level].GetTimeBetweenProjectiles();
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
