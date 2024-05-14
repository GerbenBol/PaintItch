using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject normalScreen;

    public void OnStartButton()
    {
        SceneManager.LoadScene("Main Scene");
    }

    public void OnControlsButton()
    {
        controlsScreen.SetActive(true);
        normalScreen.SetActive(false);
    }

    public void OnBackButton()
    {
        normalScreen.SetActive(true);
        controlsScreen.SetActive(false);
    }
}
