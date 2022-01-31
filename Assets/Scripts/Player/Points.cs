using System;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] float playerPoints = 0;
    [SerializeField] float pointMultiplier = 1.0f;

    public event Action onPointsChange;

    private void Start()
    {
        pointMultiplier = 1;
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

    public float GetNextLevelCost()
    {
        float nextLevelCost = pointMultiplier * 100;
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
