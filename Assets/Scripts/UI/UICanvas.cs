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

    [SerializeField] UpgradeButton healthButton = null;
    [SerializeField] TextMeshProUGUI healthCostText = null;

    [SerializeField] UpgradeButton shieldButton = null;
    [SerializeField] TextMeshProUGUI shieldCostText = null;

    [SerializeField] UpgradeButton pointsButton = null;
    [SerializeField] TextMeshProUGUI pointsCostText = null;

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

    private void UpdateUpgradeButtons()
    { 
        foreach(UpgradeButton upgradeButton in GetUpgradeButtons())
        {
            upgradeButton.GetButton().interactable = false;
            upgradeButton.SetToActive(false);
        }

        healthCostText.text = player.GetHealth().GetNextLevelCost().ToString();
        pointsCostText.text = player.GetPoints().GetNextLevelCost().ToString();
        shieldCostText.text = player.GetShield().GetNextLevelCost().ToString();

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
        pointMultiplierText.text = playerPoints.GetPointMultiplier().ToString("F1") + "x";
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
