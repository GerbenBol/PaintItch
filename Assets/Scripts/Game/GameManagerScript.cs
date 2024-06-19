using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    public static int CurrentLevel = -1;
    public static bool ReadyToLeave = false;

    private readonly static Dictionary<int, Level> levels = new();
    private static GameObject leaveBus;

    private void Start()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = 30;
        leaveBus = GameObject.Find("LeaveBus");
        leaveBus.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            canvas.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.L))
            CompleteGame();
    }

    public static void AddLevel(int id, Level level)
    {
        levels.Add(id, level);
    }

    public static int AddObject(int level, PaintableObject obj)
    {
        return levels[level].AddObject(obj);
    }

    public static void AddFence(int level, FenceOpen fence)
    {
        levels[level].AddFence(fence);
    }

    public static void AddArrow(int level, GameObject arrow)
    {
        levels[level].AddArrow(arrow);
    }

    public static double GetPrice(int level)
    {
        return levels[level].ObjectPrice;
    }

    public static void CompleteObject(int level, int index)
    {
        levels[level].CompleteObject(index);
    }

    public static void CompleteLevel()
    {
        bool allCompleted = true;

        foreach (KeyValuePair<int, Level> kvp in levels)
            if (!kvp.Value.Completed)
                allCompleted = false;

        if (allCompleted)
            CompleteGame();
    }

    public static void OpenLevel(int level)
    {
        // Check if any fences are open
        if (CurrentLevel != -1)
            levels[CurrentLevel].StopLevel();

        // Open new level
        levels[level].StartLevel();
        CurrentLevel = level;
    }

    private static void CompleteGame()
    {
        ReadyToLeave = true;
        leaveBus.SetActive(true);
    }
}
