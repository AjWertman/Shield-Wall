using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    [SerializeField] GameObject hud = null;
    [SerializeField] TextMeshProUGUI healthText = null;
    [SerializeField] Slider healthSlider = null;
    [SerializeField] TextMeshProUGUI pointsText = null;

    [SerializeField] Upgrade[] upgrades = null;

    [SerializeField] TextMeshProUGUI pointMultiplierText = null;

    [SerializeField] TextMeshProUGUI levelText = null;

    Player player = null;
    Health playerHealth = null;
    Points playerPoints = null;

    ProjectileSpawner projectileSpawner = null;

    SoundFXManager sfxManager = null;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        projectileSpawner = FindObjectOfType<ProjectileSpawner>();
        sfxManager = FindObjectOfType<SoundFXManager>();
        player.onUpgrade += MaxLevelCheck;
        projectileSpawner.onLevelUp += UpdateLevelUI;
        player.onRestart += () => ActivateHUD(true);
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

        ActivateHUD(false);
    }

    private void InitializeUpgradeButtons()
    {
        foreach (UpgradeButton upgradeButton in GetUpgradeButtons())
        {
            upgradeButton.GetButton().onClick.AddListener(() => player.Upgrade(upgradeButton.GetUpgradeType()));
            upgradeButton.GetButton().onClick.AddListener(() => UpdateUpgradeButtons());
            upgradeButton.GetButton().onClick.AddListener(() => UpdatePointsUI());
            upgradeButton.GetButton().onClick.AddListener(() => sfxManager.PlayAudioClip(Sound.LevelUp));
        }

        UpdateUpgradeButtons();
    }

    public void ActivateHUD(bool shouldActivate)
    {
        hud.SetActive(shouldActivate);
    }

    private void UpdateHealthUI()
    {
        healthText.text = playerHealth.GetHealth() + "/" + playerHealth.GetMaxHealth();
        healthSlider.value = playerHealth.GetHealthPercentage();
    }

    private void UpdatePointsUI()
    {
        pointsText.text = playerPoints.GetPlayerPoints().ToString("F0") + "g";
        pointMultiplierText.text = playerPoints.GetPointMultiplier().ToString("0.0") + "x";
    }

    private void UpdateUpgradeButtons()
    {
        foreach(Upgrade upgrade in upgrades)
        {
            float levelUpCost = GetLevelUpCost(upgrade.GetUpgradeType());
            bool canAffordUpgrade = player.CanAffordUpgrade(levelUpCost);
            string costText = "";

            if (levelUpCost != Mathf.Infinity)
            {
                costText = levelUpCost.ToString();
            }
            else
            {
                costText = "MAX";
            }

            upgrade.GetCostText().text = costText;
            upgrade.GetUpgradeButton().GetButton().interactable = canAffordUpgrade;
            upgrade.GetUpgradeButton().SetToActive(canAffordUpgrade);          
        }
    }

    public UpgradeButton GetUpgradeButton(UpgradeType upgradeType)
    {
        Upgrade upgrade = GetUpgrade(upgradeType);

        return upgrade.GetUpgradeButton();
    }

    private void UpdateLevelUI(int level, bool isDeathLevel)
    {
        if (isDeathLevel)
        {
            levelText.text = "Doom";
        }
        else
        {
            levelText.text = level.ToString();
        }        
    }

    private void MaxLevelCheck(UpgradeType upgradeType, bool isMaxLevel)
    {
        if (!isMaxLevel || upgradeType == UpgradeType.Health) return;

        GetUpgradeButton(upgradeType).GetButton().interactable = false;
    }

    public IEnumerable<UpgradeButton> GetUpgradeButtons()
    {
        foreach(Upgrade upgrade in upgrades)
        {
            yield return upgrade.GetUpgradeButton();
        }
    }

    private void DisableUpgradeButtons()
    {
        foreach (UpgradeButton upgradeButton in GetUpgradeButtons())
        {
            upgradeButton.GetButton().interactable = false;
            upgradeButton.SetToActive(false);
        }
    }

    public Upgrade GetUpgrade(UpgradeType upgradeType)
    {
        Upgrade upgrade = null;
        foreach (Upgrade _upgrade in upgrades)
        {
            if (upgradeType == upgrade.GetUpgradeType())
            {
                upgrade = _upgrade;
                break;
            }
        }

        if (upgrade == null) print("Null");

        return upgrade;
    }

    public float GetLevelUpCost(UpgradeType upgradeType)
    {
        float nextLevelCost = 0f;

        if (upgradeType == UpgradeType.Health)
        {
            nextLevelCost = player.GetHealth().GetLevelUpCost();
        }
        if (upgradeType == UpgradeType.PointsMultiplier)
        {
            nextLevelCost = player.GetPoints().GetLevelUpCost();
        }
        if (upgradeType == UpgradeType.Shield)
        {
            nextLevelCost = player.GetShield().GetNextLevelCost();
        }

        return nextLevelCost;
    }
}
