using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject normalScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject creditsScreen;

    private bool controlsActive;

    public void OnStartButton()
    {
        GameManagerScript.HasStarted = false;
        SceneManager.LoadScene("LoadingScreen");
    }

    public void OnControlsButton()
    {
        controlsScreen.SetActive(true);
        normalScreen.SetActive(false);
        controlsActive = true;
    }

    public void OnCreditsButton()
    {
        creditsScreen.SetActive(true);
        normalScreen.SetActive(false);
    }

    public void OnBackButton()
    {
        normalScreen.SetActive(true);

        if (controlsActive)
        {
            controlsActive = false;
            controlsScreen.SetActive(false);
        }
        else
            creditsScreen.SetActive(false);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
