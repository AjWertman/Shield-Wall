using System;
using UnityEngine;

[System.Serializable]
public class PointMultiplierProgression
{
    [SerializeField] int level = 0;
    [SerializeField] int nextLevelCost = 100;

    [SerializeField] float pointMultiplier = 1.0f;

    public int GetLevel()
    {
        return level;
    }

    public int GetNextLevelCost()
    {
        return nextLevelCost;
    }

    public float GetPointMultiplier()
    {
        return pointMultiplier;
    }
}

public class Points : MonoBehaviour
{
    [SerializeField] PointMultiplierProgression[] pointMultiplierProgressions = null;
    [SerializeField] int currentLevel = -1;
    int maxLevel = 0;

    [SerializeField] float playerPoints = 0;
    [SerializeField] float pointMultiplier = 1.0f;

    public event Action onPointsChange;

    private void Awake()
    {
        maxLevel = pointMultiplierProgressions.Length - 1;
    }

    private void Start()
    {
        pointMultiplier = pointMultiplierProgressions[0].GetPointMultiplier();
    }

    public void AddPoints(float pointsToAdd)
    {
        float adjustedPoints = pointsToAdd * pointMultiplier;
        playerPoints += adjustedPoints;

        onPointsChange();
    }

    public void SubtractPoints(float pointsToSubtract)
    {
        playerPoints -= pointsToSubtract;
        playerPoints = Mathf.Clamp(playerPoints, 0, Mathf.Infinity);

        onPointsChange();
    }

    public void UpgradeMultiplier()
    {
        if (IsMaxLevel()) return;
        currentLevel++;

        PointMultiplierProgression currentProgression = GetProgression(currentLevel);

        pointMultiplier = currentProgression.GetPointMultiplier();
    }

    public int GetNextLevelCost()
    {
        return GetProgression(currentLevel).GetNextLevelCost();
    }

    public bool IsMaxLevel()
    {
        return currentLevel == maxLevel;
    }

    public float GetPlayerPoints()
    {
        return playerPoints;
    }

    public float GetPointMultiplier()
    {
        return pointMultiplier;
    }

    public PointMultiplierProgression GetProgression(int level)
    {
        PointMultiplierProgression progression = null;

        foreach (PointMultiplierProgression pointMultiplierProgression in pointMultiplierProgressions)
        {
            if (pointMultiplierProgression.GetLevel() == level)
            {
                progression = pointMultiplierProgression;
            }
        }

        return progression;
    }
}
