using System;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] UpgradeButton healthButton = null;
    [SerializeField] UpgradeButton shieldButton = null;
    [SerializeField] UpgradeButton pointsButton = null;

    Player player = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        player.onUpgrade += MaxLevelCheck;
    }

    private void Start()
    {
        InitializeUpgradeButtons();

        player.GetPoints().onPointsChange += UpdatePointsUI;
        UpdatePointsUI();
    }

    private void InitializeUpgradeButtons()
    {
        foreach (UpgradeButton upgradeButton in GetUpgradeButtons())
        {
            upgradeButton.GetButton().onClick.AddListener(() => player.Upgrade(upgradeButton.GetUpgradeType()));
            upgradeButton.GetButton().onClick.AddListener(() => UpdateUpgradeButtons());
        }
    }

    private void UpdatePointsUI()
    {
        UpdateUpgradeButtons();
        //Points ui.text = newamount
    }

    private void UpdateUpgradeButtons()
    {
        foreach(UpgradeButton upgradeButton in GetUpgradeButtons())
        {
            upgradeButton.GetButton().interactable = false;
        }

        if (player.CanAffordUpgrade(UpgradeType.Health))
        {
            healthButton.GetButton().interactable = true;
        }
        if (player.CanAffordUpgrade(UpgradeType.PointsMultiplier))
        {
            pointsButton.GetButton().interactable = true;
        }
        if (player.CanAffordUpgrade(UpgradeType.Shield))
        {
            shieldButton.GetButton().interactable = true;
        }
    }

    public UpgradeButton GetUpgradeButton(UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.Health)
        {
            return healthButton;
        }
        else if (upgradeType == UpgradeType.PointsMultiplier)
        {
            return shieldButton;
        }
        else if (upgradeType == UpgradeType.Shield)
        {
            return pointsButton;
        }

        return null;
    }

    private void MaxLevelCheck(UpgradeType upgradeType, bool isMaxLevel)
    {
        if (!isMaxLevel) return;

        GetUpgradeButton(upgradeType).GetButton().interactable = false;
    }

    public IEnumerable<UpgradeButton> GetUpgradeButtons()
    {
        yield return healthButton;
        yield return shieldButton;
        yield return pointsButton;
    }
}
