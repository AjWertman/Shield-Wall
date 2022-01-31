using System;
using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] ProjectilesSpawnProgression[] projectilesSpawnProgressions = null;
    [SerializeField] ProjectilesSpawnProgression deathMode = null;

    [SerializeField] float timeBetweenProjectiles = 2f;
    [SerializeField] float secondsBetweenLevelUp = 30f;
    [SerializeField] float secondsUntilDeathMode = 300f;

    public ProjectilesSpawnProgression currentProgression = null;
    ProjectilePool projectilePool = null;

    bool canSpawnProjectiles = true;

    float minYClamp = 0f;
    float maxYClamp = 0f;

    int level = 0;
    int maxLevel = 0;

    float gameTimer = 0f;

    bool hasStartedLevelUp = false;

    public event Action<int, bool> onLevelUp;

    private void Awake()
    {
        projectilePool = GetComponent<ProjectilePool>();
        SetLauncherClamps();

        maxLevel = projectilesSpawnProgressions.Length - 1;
        currentProgression = projectilesSpawnProgressions[0];   
    }

    private void Start()
    {
        StartCoroutine(StartDeathModeCountdown());
        currentProgression = GetProjectileSpawnProgression();
    }

    private void Update()
    {
        if (canSpawnProjectiles)
        {
            canSpawnProjectiles = false;
            StartCoroutine(LaunchProjectile());
        }
        
        gameTimer += Time.deltaTime;

        if (hasStartedLevelUp || level >= maxLevel) return;
        StartCoroutine(Progression());
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

    private IEnumerator Progression()
    {
        hasStartedLevelUp = true;
        yield return new WaitForSeconds(secondsBetweenLevelUp);
        AdvanceLevel();
        hasStartedLevelUp = false;
    }

    public void AdvanceLevel()
    {
        if (level + 1 > maxLevel) return;

        level++;

        currentProgression = GetProjectileSpawnProgression();
        timeBetweenProjectiles = currentProgression.GetTimeBetweenProjectiles();
        onLevelUp(level, false);
    }

    private IEnumerator StartDeathModeCountdown()
    {
        yield return new WaitForSeconds(secondsUntilDeathMode);
        currentProgression = deathMode;
        timeBetweenProjectiles = currentProgression.GetTimeBetweenProjectiles();

        onLevelUp(0, true);
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

        foreach(ProjectileSpawnChances projectileSpawnChance in currentProgression.GetProjectileSpawnChances())
        {
            Vector2 chances = projectileSpawnChance.GetChances();

            if(chances.x < randomArrowInt && randomArrowInt < chances.y)
            {
                return projectileSpawnChance.GetProjectileType();
            }
        }

        return ProjectileType.rArrow;
    }

    private ProjectilesSpawnProgression GetProjectileSpawnProgression()
    {
        ProjectilesSpawnProgression progression = null;

        foreach (ProjectilesSpawnProgression projectilesSpawnProgression in projectilesSpawnProgressions)
        {
            if (projectilesSpawnProgression.GetLevel() == level)
            {
                progression = projectilesSpawnProgression;
            }
        }

        return progression;
    }
}

[Serializable]
public class ProjectilesSpawnProgression
{
    [SerializeField] int level = 0;
    [SerializeField] float timeBetweenProjectiles = 2f;
    [SerializeField] ProjectileSpawnChances[] projectileSpawnChances = null;

    public int GetLevel()
    {
        return level;
    }

    public float GetTimeBetweenProjectiles()
    {
        return timeBetweenProjectiles;
    }

    public ProjectileSpawnChances[] GetProjectileSpawnChances()
    {
        return projectileSpawnChances;
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
