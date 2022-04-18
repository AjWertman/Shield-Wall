using System;
using TMPro;
using UnityEngine;

public enum UpgradeType { Health, Shield, PointsMultiplier }

[Serializable]
public class Upgrade
{
    [SerializeField] UpgradeType upgradeType = UpgradeType.Health;
    [SerializeField] UpgradeButton upgradeButton = null;
    [SerializeField] TextMeshProUGUI costText = null;

    public UpgradeType GetUpgradeType()
    {
        return upgradeType;
    }
    
    public UpgradeButton GetUpgradeButton()
    {
        return upgradeButton;
    }

    public TextMeshProUGUI GetCostText()
    {
        return costText;
    }
}
