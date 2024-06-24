using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject controlsScreen;

    public void OnResumeButton()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canvas.SetActive(false);
        Time.timeScale = 1.0f;
        GameManagerScript.InMenu = false;
    }

    public void OnControlsButton()
    {
        controlsScreen.SetActive(true);
        mainScreen.SetActive(false);
    }

    public void OnBackButton()
    {
        mainScreen.SetActive(true);
        controlsScreen.SetActive(false);
    }

    public void OnMainMenuButton()
    {
        GameManagerScript.RestartGame();
        SceneManager.LoadScene("Temp Main Menu");
    }
}
