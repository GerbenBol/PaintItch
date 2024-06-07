using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int LevelID { get => levelID; }
    public bool Completed { get => completed; }
    public string Quest;

    private readonly Dictionary<int, bool> objects = new();
    private readonly int levelID;
    private int index = 0;
    private bool completed = false;
    private FenceOpen levelFence;

    private void Start()
    {
        
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
}
