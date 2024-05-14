using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private static readonly Dictionary<int, bool> paintableObjects = new();
    private static int index = 0;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            canvas.SetActive(true);
        }

        //Temp win condition
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 1)
        {
            SceneManager.LoadScene("End Level");
        }
    }

    public static int AddObject()
    {
        paintableObjects.Add(index, false);
        index++;
        return index - 1;
    }

    public static void CompleteObject(int index)
    {
        paintableObjects[index] = true;

        bool allCompleted = true;

        foreach (KeyValuePair<int, bool> kvp in paintableObjects)
            if (!kvp.Value)
                allCompleted = false;

        if (allCompleted)
            SceneManager.LoadScene("End Level");
    }
}
