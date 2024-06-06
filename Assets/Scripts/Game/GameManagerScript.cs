using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    public static int CurrentLevel = -1;

    private readonly static Dictionary<int, Level> levels = new();

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

    public static int AddObject(int level)
    {
        // Check if the level already exists, if not make new
        if (levels.ContainsKey(level))
            return levels[level].AddObject();
        else
            levels.Add(level, new(level, 0));

        return 0;
    }

    public static void AddFence(int level, FenceOpen fence)
    {
        levels[level].AddFence(fence);
    }

    public static void CompleteObject(int level, int index)
    {
        levels[level].CompleteObject(index);
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
}
