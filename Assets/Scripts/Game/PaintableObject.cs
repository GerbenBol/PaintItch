using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    public bool required;
    public bool ExtraObject = false;
    public Color ExtraColor;

    public float completionPercentage = 1f;
    public float completedPercentage = .0f;
    public Texture2D MainTexture;
    [SerializeField] private Texture2D aoTexture;

    private bool completed = false;
    private float notBlack = 0;
    private int index, indexTotal = 0;
    private int pixelIndexX = 0, pixelIndexY = 0;
    private int textureSize;
    private int level;
    private readonly int circleSize = 4;

    private readonly Dictionary<int, int> circle = new()
    {
        { -11, 7 }, { -10, 9 }, { -9, 10 }, { -8, 12 }, { -7, 12 }, { -6, 13 },
        { -5, 14 }, { -4, 14 }, { -3, 15 }, { -2, 15 }, { -1, 15 }, { 0, 15 },
        { 1, 15 }, { 2, 15 }, { 3, 15 }, { 4, 14 }, { 5, 14 }, { 6, 13 },
        { 7, 12 }, { 8, 12 }, { 9, 10 }, { 10, 9 }, { 11, 7 }
    };
    private readonly Dictionary<Vector2Int, Color> updateList = new();
    private bool[,] pixelsUpdated;

    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        MainTexture = Instantiate(rend.material.mainTexture) as Texture2D;
        rend.material.mainTexture = MainTexture;
        pixelsUpdated = new bool[MainTexture.width, MainTexture.height];
        textureSize = MainTexture.width * MainTexture.height;
        level = Convert.ToInt32(tag.Substring(tag.Length - 1, 1));

        if (required)
        {
            TextureControl.ToCalculate.Add(this);
            index = GameManagerScript.AddObject(level, this);
        }
    }

    public int CalculatePercentage(int checkAmount)
    {
        int currentChecked = 0;
        int xBreak = 0, yBreak = 0;

        while (pixelIndexX < aoTexture.width)
        {
            while (pixelIndexY < aoTexture.height)
            {
                if (aoTexture.GetPixel(pixelIndexX, pixelIndexY) != Color.black)
                    notBlack++;

                pixelIndexY++;
                indexTotal++;
                currentChecked++;

                if (currentChecked >= checkAmount)
                {
                    xBreak = pixelIndexX;
                    yBreak = pixelIndexY;
                    break;
                }
            }

            pixelIndexY = 0;
            pixelIndexX++;
        }

        pixelIndexX = xBreak;
        pixelIndexY = yBreak;

        if (indexTotal >= textureSize)
        {
            float paintablePercentage = notBlack / (aoTexture.width * aoTexture.height);
            completionPercentage = paintablePercentage * .85f;
            Debug.Log(name);
            checkAmount -= currentChecked;
        }
        else
            checkAmount = -1;

        return checkAmount;
    }

    public void ChangeTexture(Vector2Int coords, Color color)
    {
        updateList.Add(coords, color);
        StartCoroutine(nameof(UpdateColor));
    }

    private void CheckExtra()
    {
        int correctColor = 0;

        for (int x = 0; x < MainTexture.width; x++)
            for (int y = 0; y < MainTexture.height; y++)
                if (MainTexture.GetPixel(x, y) == ExtraColor)
                    correctColor++;

        if (correctColor >= textureSize * completionPercentage * .85f)
            Debug.Log("correct color");
        else
            Debug.Log("incorrect color");
    }

    private IEnumerator UpdateColor()
    {
        foreach (KeyValuePair<Vector2Int, Color> kvp in updateList)
        {
            float changedPixels = 0;
            int firstIndex = 0, lastIndex = 0;

            foreach (KeyValuePair<int, int> coords in circle)
            {
                if (firstIndex == 0)
                    firstIndex = coords.Key;

                lastIndex = coords.Key;
            }

            for (int x = firstIndex * circleSize; x < lastIndex * circleSize; x++)
                for (int y = -circle[x / circleSize] * circleSize; y <= circle[x / circleSize] * circleSize; y++)
                {
                    Vector2Int vec2 = new(kvp.Key.x + x, kvp.Key.y + y);
                    MainTexture.SetPixel(vec2.x, vec2.y, kvp.Value);

                    if (vec2.x < MainTexture.width && vec2.x >= 0 && vec2.y < MainTexture.height && vec2.y >= 0)
                        if (!pixelsUpdated[vec2.x, vec2.y])
                        {
                            pixelsUpdated[vec2.x, vec2.y] = true;
                            changedPixels++;
                        }
                }

            float totalPixels = MainTexture.width * MainTexture.height;
            completedPercentage += changedPixels / totalPixels;

            if (completedPercentage >= completionPercentage && !completed)
            {
                completed = true;
                GameManagerScript.CompleteObject(level, index);
                AudioSource.PlayClipAtPoint(TextureControl.CompletedDing, transform.position);

                if (ExtraObject)
                    CheckExtra();
            }

            if (!TextureControl.ToUpdate.Contains(MainTexture))
                TextureControl.ToUpdate.Add(MainTexture);
        }
        updateList.Clear();
        yield return null;
    }
}
