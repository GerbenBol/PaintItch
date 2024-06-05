using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    public static int CurrentLevel = -1;
    public static List<FenceOpen> LevelFences = new();

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

    public static void OpenLevel(int level)
    {
        if (CurrentLevel != -1)
            LevelFences[CurrentLevel].Close();

        LevelFences[level].Open();
        CurrentLevel = level;
    }
}
