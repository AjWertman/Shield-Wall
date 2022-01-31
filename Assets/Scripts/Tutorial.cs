using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] List<GameObject> phases = new List<GameObject>();
    [SerializeField] GameObject clickToContinue = null;

    bool hasCompletedTutorial = false;
    bool isActive = false;

    Image image = null;

    public event Action onTutorialComplete;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        ActivateTutorial(false);
    }

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            OnNextPageButton();
        }
    }

    public void ActivateTutorial(bool shouldActivate)
    {
        if (shouldActivate)
        {
            gameObject.SetActive(true);
            ActivatePage(phases[0]);        
        }
        else
        {           
            DisableAllPages();
            gameObject.SetActive(false);
        }

        clickToContinue.SetActive(shouldActivate);
        image.enabled = shouldActivate;
        isActive = shouldActivate;
    }

    private void OnNextPageButton()
    {
        GameObject nextPage = GetNextPage();

        if(nextPage != null)
        {
            ActivatePage(nextPage);
        }
        else
        {
            CompleteTutorial();
        }
    }

    private void ActivatePage(GameObject pageToActivate)
    {
        DisableAllPages();
        pageToActivate.SetActive(true);
    }

    private void CompleteTutorial()
    {
        hasCompletedTutorial = true;
        onTutorialComplete();
        ActivateTutorial(false);
    }

    public bool HasCompletedTutorial()
    {
        return hasCompletedTutorial;
    }

    private void DisableAllPages()
    {
        foreach (GameObject page in phases)
        {
            page.SetActive(false);
        }
    }

    private GameObject GetNextPage()
    {
        GameObject nextPage = null;

        int currentPageIndex = phases.IndexOf(GetCurrentPage());
        int nextPageIndex = currentPageIndex + 1;

        if(nextPageIndex < phases.Count)
        {
            nextPage = phases[nextPageIndex];
        }

        return nextPage;
    }

    private GameObject GetCurrentPage()
    {
        GameObject currentPage = null;
        foreach (GameObject page in phases)
        {
            if (page.activeSelf)
            {
                currentPage = page;
            }
        }

        return currentPage;
    } 
}
