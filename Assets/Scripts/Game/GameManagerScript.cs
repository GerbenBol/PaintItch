using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    public static int CurrentLevel = -1;
    public static bool ReadyToLeave = false;
    public static bool InMenu = false;

    private readonly static Dictionary<int, Level> levels = new();
    private static GameObject leaveBus;
    private static GameObject bus;
    private static GameObject player;
    private static GameObject finalCamera;
    private static GameObject standardUI;
    private static GameObject endUI;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !InMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            canvas.SetActive(true);
            InMenu = true;
        }
        else if (canvas.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            canvas.SetActive(false);
            InMenu = false;
        }
        

        if (Input.GetKeyDown(KeyCode.L))
            CompleteGame();
    }

    public static void RestartGame()
    {
        levels.Clear();
        StartGame();
    }

    public static void StartGame()
    {
        CurrentLevel = -1;
        ReadyToLeave = false;
        InMenu = false;
        Time.timeScale = 1f;
        Application.targetFrameRate = 30;
        leaveBus = GameObject.Find("LeaveBus");
        bus = GameObject.Find("P_Bus");
        player = GameObject.Find("Player");
        finalCamera = GameObject.Find("FinalCamera");
        standardUI = GameObject.Find("IngameUI");
        endUI = GameObject.Find("EndUI");
        leaveBus.SetActive(false);
        endUI.SetActive(false);
        finalCamera.SetActive(false);
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

    public static void StartFinalCamera()
    {
        endUI.SetActive(true);
        finalCamera.SetActive(true);
        bus.SetActive(false);
        player.SetActive(false);
        standardUI.SetActive(false);
        InMenu = true;
    }

    private static void CompleteGame()
    {
        ReadyToLeave = true;
        leaveBus.SetActive(true);
        leaveBus.transform.GetChild(0).gameObject.SetActive(true);
    }
}
