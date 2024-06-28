using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void OnMainMenuButton()
    {
        SceneManager.LoadSceneAsync("Temp Main Menu");
    }

    public void OnRetryButton()
    {
        GameManagerScript.RestartGame();
        SceneManager.LoadSceneAsync("Main 2");
    }
}
