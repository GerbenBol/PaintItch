using System.Collections;
using System.Collections.Generic;

public class Level
{
    public int LevelID { get => levelID; }
    public bool Completed { get => completed; }

    private readonly Dictionary<int, bool> objects = new();
    private readonly int levelID;
    private int index = 0;
    private bool completed = false;
    private FenceOpen levelFence;

    public Level(int id, int firstItem)
    {
        levelID = id;
        objects.Add(firstItem, false);
        index++;
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
