using System;
using UnityEngine;

[System.Serializable]
public class ShieldProgression
{
    [SerializeField] int level = 0;
    [SerializeField] float nextLevelCost = 100;

    [SerializeField] float shieldSpeed = 5f;

    public int GetLevel()
    {
        return level;
    }

    public float GetNextLevelCost()
    {
        return nextLevelCost;
    }

    public float GetShieldSpeed()
    {
        return shieldSpeed;
    }
}

public class Shield : MonoBehaviour
{
    [SerializeField] ShieldProgression[] shieldProgressions = null;
    [SerializeField] int currentLevel = -1;
    int maxLevel = 0;

    [SerializeField] Vector3 startPosition = Vector3.zero;
    [SerializeField] Vector2 yClamps = Vector2.zero;

    [SerializeField] float moveSpeed = 5f;

    public event Action<Projectile> onProjectileHit;

    bool canMove = false;

    private void Awake()
    {
        maxLevel = shieldProgressions.Length - 1;
    }

    private void Start()
    {
        transform.position = startPosition;

        moveSpeed = shieldProgressions[0].GetShieldSpeed();
    }

    private void Update()
    {
        if (!canMove) return;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            MoveShield(Vector3.up);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            MoveShield(Vector3.down);
        }
    }
    
    private void MoveShield(Vector3 direction)
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        Vector3 newPosition = transform.position;

        newPosition.y = Mathf.Clamp(newPosition.y, GetMinYClamp(), GetMaxYClamp());
        transform.position = newPosition;
    }

    public void Upgrade()
    {
        if (IsMaxLevel()) return;
        currentLevel++;

        ShieldProgression currentProgression = GetProgression(currentLevel);

        moveSpeed = currentProgression.GetShieldSpeed();
        //deactivate all shield objects - activate the shield to activate
    }

    public float GetNextLevelCost()
    {
        ShieldProgression nextProgression = GetProgression(currentLevel);

        return nextProgression.GetNextLevelCost();
    }

    public bool IsMaxLevel()
    {
        return currentLevel == maxLevel;
    }

    public void Restart()
    {
        currentLevel = 0;
        transform.position = startPosition;

        moveSpeed = shieldProgressions[0].GetShieldSpeed();

        canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile == null) return;

        onProjectileHit(projectile);
    }

    public void SetCanMove(bool shouldSet)
    {
        canMove = shouldSet;
    }

    public ShieldProgression GetProgression(int level)
    {
        ShieldProgression progression = null;

        foreach (ShieldProgression shieldProgression in shieldProgressions)
        {
            if (shieldProgression.GetLevel() == level)
            {
                progression = shieldProgression;
            }
        }
        return progression;
    }

    public float GetMinYClamp()
    {
        return yClamps.x;
    }
    public float GetMaxYClamp()
    {
        return yClamps.y;
    }
}
