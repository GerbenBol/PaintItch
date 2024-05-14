using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void OnMainMenuButton()
    {
        SceneManager.LoadScene("Temp Main Menu");
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene("Main Scene");
    }
}
