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

    ProjectilesSpawnProgression currentProgression = null;
    ProjectilePool projectilePool = null;

    SoundFXManager sfxManager = null;

    bool canSpawnProjectiles = true;
    bool isActivated = false;

    float minYClamp = 0f;
    float maxYClamp = 0f;

    int level = 0;
    int maxLevel = 0;

    bool hasStartedLevelUp = false;

    bool isDead = false;

    public event Action<int, bool> onLevelUp;

    private void Awake()
    {
        projectilePool = FindObjectOfType<ProjectilePool>();
        sfxManager = FindObjectOfType<SoundFXManager>();
        SetLauncherClamps();

        maxLevel = projectilesSpawnProgressions.Length - 1;
    }

    private void Update()
    {
        if (!isActivated) return;
        if (canSpawnProjectiles)
        {
            canSpawnProjectiles = false;
            StartCoroutine(LaunchProjectile());
        }

        if (hasStartedLevelUp || level >= maxLevel) return;
        StartCoroutine(Progression());
    }

    public void Activate()
    {
        projectilePool.CallBackAllProjectiles();
        level = 1;
        onLevelUp(level, false);

        currentProgression = GetProjectileSpawnProgression();
        StartCoroutine(StartDeathModeCountdown());
        isActivated = true;
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

        sfxManager.PlayAudioClip(newProjectile.GetLaunchSound());

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

    public void DeathBehavior()
    {
        isDead = true;
    }

    private ProjectileType GetRandomProjectileType()
    {
        int randomArrowInt = UnityEngine.Random.Range(0, 100);

        if (isDead)
        {
            return GetArrowsOnlyProjectileType();
        }

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

    private ProjectileType GetArrowsOnlyProjectileType()
    {
        int random = UnityEngine.Random.Range(0, 3);

        if(random == 0)
        {
            return ProjectileType.lArrow;
        }
        else if(random == 1)
        {
            return ProjectileType.rArrow;
        }
        else
        {
            return ProjectileType.hArrow;
        }
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
