using System;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] float playerPoints = 0;
    [SerializeField] float pointMultiplier = 1.0f;

    public event Action onPointsChange;

    private void Start()
    {
        pointMultiplier = 1.0f;
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

        pointMultiplier += .2f;
    }

    public void Restart()
    {
        pointMultiplier = 1;
        playerPoints = 0;

        onPointsChange();
    }

    public float GetLevelUpCost()
    {
        float nextLevelCost = 0;
        if (!IsMaxLevel())
        {
            nextLevelCost = Mathf.Round(pointMultiplier * 100f);
        }
        else
        {
            nextLevelCost = Mathf.Infinity;
        }
        
        return nextLevelCost;
    }

    public bool IsMaxLevel()
    {
        return pointMultiplier == 100;
    }

    public float GetPlayerPoints()
    {
        return playerPoints;
    }

    public float GetPointMultiplier()
    {
        return pointMultiplier;
    }
}
