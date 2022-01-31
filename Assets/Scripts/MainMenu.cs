using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button startButton = null;
    [SerializeField] Button creditsButton = null;
    [SerializeField] Button quitButton =null;

    [SerializeField] GameObject creditsPage = null;
    [SerializeField] Button creditsBackButton = null;

    Tutorial tutorial = null;
    UICanvas uicanvas = null;

    private void Awake()
    {
        tutorial = FindObjectOfType<Tutorial>();
        uicanvas = GetComponentInParent<UICanvas>();

        startButton.onClick.AddListener(StartGame);
        creditsButton.onClick.AddListener(()=> ActivateCreditsPage(true));
        quitButton.onClick.AddListener(() => Application.Quit());

        creditsBackButton.onClick.AddListener(() => ActivateCreditsPage(false));

        ActivateCreditsPage(false);
    }

    private void StartGame()
    {
        tutorial.ActivateTutorial(true);
        uicanvas.ActivateHUD(true);
        gameObject.SetActive(false);
    }

    private void ActivateCreditsPage(bool shouldActivate)
    {
        creditsPage.SetActive(shouldActivate);
    }
}
