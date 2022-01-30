using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText = null;
    [SerializeField] Slider healthSlider = null;

    [SerializeField] TextMeshProUGUI pointsText = null;

    [SerializeField] UpgradeButton healthButton = null;
    [SerializeField] UpgradeButton shieldButton = null;
    [SerializeField] UpgradeButton pointsButton = null;
    [SerializeField] TextMeshProUGUI pointMultiplierText = null;

    Player player = null;
    Health playerHealth = null;
    Points playerPoints = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        player.onUpgrade += MaxLevelCheck;
    }

    private void Start()
    {
        playerHealth = player.GetHealth();
        playerPoints = player.GetPoints();

        InitializeUpgradeButtons();

        playerHealth.onHealthChange += UpdateHealthUI;

        playerPoints.onPointsChange += UpdatePointsUI;
        playerPoints.onPointsChange += UpdateUpgradeButtons;

        UpdateHealthUI();
        UpdateUpgradeButtons();
        UpdatePointsUI();
    }

    private void InitializeUpgradeButtons()
    {
        foreach (UpgradeButton upgradeButton in GetUpgradeButtons())
        {
            upgradeButton.GetButton().onClick.AddListener(() => player.Upgrade(upgradeButton.GetUpgradeType()));
            upgradeButton.GetButton().onClick.AddListener(() => UpdateUpgradeButtons());
            upgradeButton.GetButton().onClick.AddListener(() => UpdatePointsUI());
        }
    }

    private void UpdateHealthUI()
    {
        healthText.text = playerHealth.GetHealth() + "/" + playerHealth.GetMaxHealth();
        healthSlider.value = playerHealth.GetHealthPercentage();
    }

    private void UpdateUpgradeButtons()
    {
        foreach(UpgradeButton upgradeButton in GetUpgradeButtons())
        {
            upgradeButton.GetButton().interactable = false;
            upgradeButton.SetToActive(false);
        }

        if (player.CanAffordUpgrade(UpgradeType.Health))
        {
            healthButton.GetButton().interactable = true;
            healthButton.SetToActive(true);
        }
        if (player.CanAffordUpgrade(UpgradeType.PointsMultiplier))
        {
            pointsButton.GetButton().interactable = true;
            pointsButton.SetToActive(true);
        }
        if (player.CanAffordUpgrade(UpgradeType.Shield))
        {
            shieldButton.GetButton().interactable = true;
            shieldButton.SetToActive(true);
        }
    }

    private void UpdatePointsUI()
    {
        pointsText.text = playerPoints.GetPlayerPoints().ToString("F0") + "g";
        pointMultiplierText.text = playerPoints.GetPointMultiplier().ToString() + "x";
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
