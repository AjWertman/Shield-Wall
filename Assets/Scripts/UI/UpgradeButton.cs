using UnityEngine;
using UnityEngine.UI;

public enum UpgradeType { Health, Shield, PointsMultiplier}

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UpgradeType upgradeType = UpgradeType.Health;

    Button button = null;
    CanvasGroup canvasGroup = null;

    private void Awake()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetToActive(bool shouldSet)
    {
        if (shouldSet)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = .5f;
        }
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
