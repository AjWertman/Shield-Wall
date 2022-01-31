using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText = null;
    [SerializeField] GameObject buttonGroup = null;
    [SerializeField] Button playAgainButton = null;
    [SerializeField] Button quitButton = null;

    [SerializeField] float fadeOutTime = 2f;

    CanvasGroup canvasGroup = null;
    UICanvas uiCanvas = null;

    public event Action onRestartButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        uiCanvas = FindObjectOfType<UICanvas>();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    private void Start()
    {
        SetupButtons();
    }

    public IEnumerator InitiateDeathSequence(float score)
    {
        canvasGroup.blocksRaycasts = true;
        scoreText.text = score.ToString("F0") + "!";
        uiCanvas.ActivateHUD(false);
        yield return FadeOut(fadeOutTime);
        buttonGroup.SetActive(true);
    }

    public IEnumerator FadeOut(float time)
    {
        while(canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    private void SetupButtons()
    {
        buttonGroup.SetActive(true);
        playAgainButton.onClick.AddListener(PlayAgain);
        quitButton.onClick.AddListener(Quit);
    }

    private void PlayAgain()
    {
        canvasGroup.alpha = 0;
        uiCanvas.gameObject.SetActive(true);
        onRestartButton();
    }

    private void Quit()
    {
        Application.Quit();
    }
}
