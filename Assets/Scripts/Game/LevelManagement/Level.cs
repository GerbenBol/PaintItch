using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Level : MonoBehaviour
{
    public int LevelID { get => levelID; }
    public bool Completed { get => completed; }
    public string Quest;

    private readonly Dictionary<int, bool> objects = new();
    private int levelID;
    private int index = 0;
    private bool completed = false;
    private FenceOpen levelFence;

    private void Awake()
    {
        levelID = Convert.ToInt32(name.Substring(name.Length - 1, 1));
        GameManagerScript.AddLevel(levelID, this);
    }

    public int AddObject()
    {
        objects.Add(index, false);
        index++;
        return index - 1;
    }

    public void AddFence(FenceOpen fence)
    {
        levelFence = fence;
    }

    public void StartLevel()
    {
        // Open fence
        levelFence.Open();
    }

    public void StopLevel()
    {
        // Close fence
        levelFence.Close();
    }

    public void CompleteObject(int index)
    {
        // Set object to completed
        objects[index] = true;
        bool allCompleted = true;

        // Check if all objects in this level are completed
        foreach (KeyValuePair<int, bool> kvp in objects)
            if (!kvp.Value)
                allCompleted = false;

        if (allCompleted)
            completed = true;
    }

    public void Enlarge()
    {
        Debug.Log("enlarge " + name);
    }

    public void OriginalSize()
    {
        Debug.Log("return to original size " + name);
    }
}
