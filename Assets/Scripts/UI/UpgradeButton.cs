using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType { Health, Shield, PointsMultiplier}

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UpgradeType upgradeType = UpgradeType.Health;

    Button button = null;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public UpgradeType GetUpgradeType()
    {
        return upgradeType;
    }

    public Button GetButton()
    {
        return button;
    }
}
