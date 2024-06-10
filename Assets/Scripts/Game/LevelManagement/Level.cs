using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int LevelID { get => levelID; }
    public bool Completed { get => completed; }

    [SerializeField] private string Name;
    [SerializeField] private string QuestDesc;
    [SerializeField] private double Price;
    [SerializeField] private Color LevelColor;
    [SerializeField] private string Extra = "I would like ";

    private readonly Dictionary<int, bool> completedObjects = new();
    private readonly Dictionary<int, PaintableObject> objects = new();
    private int levelID;
    private int index = 0;
    private readonly float maxTimer = .2f;
    private float timer = .0f;
    private bool completed = false;
    private FenceOpen levelFence;
    private FenceOpen secondFence;
    private PaintableObject vio;
    private PlayerPainting player;
    private GameObject levelArrow;

    private void Awake()
    {
        levelID = Convert.ToInt32(name.Substring(name.Length - 1, 1));
        GameManagerScript.AddLevel(levelID, this);
        player = GameObject.Find("Player").GetComponent<PlayerPainting>();
    }

    private void Update()
    {
        if (timer < maxTimer)
            timer += Time.deltaTime;
        else if (name != "level0" && vio == null)
        {
            System.Random rand = new();
            int id = rand.Next(0, completedObjects.Count);
            int color = rand.Next(0, 19);
            vio = objects[id];
            vio.extraObject = true;
            string name = vio.name;
            Extra += name[..1].ToUpper() + name[1..] +
                $" to be painted {player.ColorNames[color]}.";
            LevelColor = player.Colors[color];
        }
    }

    public int AddObject(PaintableObject obj)
    {
        objects.Add(index, obj);
        completedObjects.Add(index, false);
        index++;
        return index - 1;
    }

    public void AddFence(FenceOpen fence)
    {
        if (levelFence == null)
            levelFence = fence;
        else
            secondFence = fence;
    }

    public void AddArrow(GameObject arrow)
    {
        levelArrow = arrow;
    }

    public void StartLevel()
    {
        // Open fence
        levelFence.Open();
        levelArrow.SetActive(true);

        if (secondFence != null)
            secondFence.Open();
    }

    public void StopLevel()
    {
        // Close fence
        levelFence.Close();
        levelArrow.SetActive(false);

        if (secondFence != null)
            secondFence.Close();
    }

    public void CompleteObject(int index)
    {
        // Set object to completed
        completedObjects[index] = true;
        bool allCompleted = true;

        // Check if all objects in this level are completed
        foreach (KeyValuePair<int, bool> kvp in completedObjects)
            if (!kvp.Value)
                allCompleted = false;

        if (allCompleted)
            completed = true;
    }

    public void Enlarge()
    {
        if (name != "level0")
        {
            transform.localScale = new(.09f, .09f, .09f);
            Quest.OpenQuest(Name, Price, QuestDesc, Extra, levelID, LevelColor);
        }
    }

    public void OriginalSize()
    {
        if (name != "level0")
        {
            transform.localScale = new(.07f, .07f, .07f);
            Quest.CloseQuest();
        }
    }
}
