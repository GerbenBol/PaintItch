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
    private readonly int sizeModifier = 4;
    private PlayerPainting player;

    private readonly Dictionary<int, int> eclipse = new()
    {
        { -11, 7 }, { -10, 9 }, { -9, 10 }, { -8, 12 }, { -7, 12 }, { -6, 13 },
        { -5, 14 }, { -4, 14 }, { -3, 15 }, { -2, 15 }, { -1, 15 }, { 0, 15 },
        { 1, 15 }, { 2, 15 }, { 3, 15 }, { 4, 14 }, { 5, 14 }, { 6, 13 },
        { 7, 12 }, { 8, 12 }, { 9, 10 }, { 10, 9 }, { 11, 7 }
    };
    private readonly Dictionary<int, int> square = new()
    {
        { -11, 15 }, { -10, 15 }, { -9, 15 }, { -8, 15 }, { -7, 15 },
        { -6, 15 }, { -5, 15 }, { -4, 15 }, { -3, 15 }, { -2, 15 },
        { -1, 15 }, { 0, 15 }, { 1, 15 }, { 2, 15 }, { 3, 15 },
        { 4, 15 }, { 5, 15 }, { 6, 15 }, { 7, 15 }, { 8, 15 },
        { 9, 15 }, { 10, 15 }, { 11, 15 }, { 12, 15 }, { 13, 15 },
        { 14, 15 }, { 15, 15 }
    };
    private readonly Dictionary<int, int> triangle = new()
    {
        { -11, 1 }, { -10, 1 }, { -9, 2 }, { -8, 2 }, { -7, 3 }, { -6, 4 },
        { -5, 5 }, { -4, 5 }, { -3, 6 }, { -2, 6 }, { -1, 7 }, { 0, 8 },
        { 1, 9 }, { 2, 9 }, { 3, 10 }, { 4, 10 }, { 5, 11 }, { 6, 12 },
        { 7, 13 }, { 8, 13 }, { 9, 14 }, { 10, 14 }, { 11, 15 }
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
        player = GameObject.Find("Player").GetComponent<PlayerPainting>();

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

                if (indexTotal >= textureSize)
                    break;
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
        /*int correctColor = 0;

        for (int x = 0; x < MainTexture.width; x++)
            for (int y = 0; y < MainTexture.height; y++)
            {
                if (MainTexture.GetPixel(x, y) == ExtraColor)
                    correctColor++;
                else
                    Debug.Log(MainTexture.GetPixel(x, y));
            }

        if (correctColor >= textureSize * completionPercentage * .65f)
            Debug.Log($"correct color, {correctColor}, {textureSize * completionPercentage * .65f}");
        else
            Debug.Log($"incorrect color, {correctColor}, {textureSize * completionPercentage * .65f}");*/
        if (player.Colors[player.ActiveColor] == ExtraColor)
            Shop.Money += GameManagerScript.GetPrice(level) * level;
    }

    private IEnumerator UpdateColor()
    {
        Dictionary<int, int> activeBrush;
        string brushName = PlayerPainting.brushes[PlayerPainting.brushIndex];

        if (brushName == "eclipse")
            activeBrush = eclipse;
        else if (brushName == "square")
            activeBrush = square;
        else if (brushName == "triangle")
            activeBrush = triangle;
        else
        {
            activeBrush = new();
            System.Random rand = new();
            int j = 0;

            for (int i = -11; i < 12; i++)
            {
                if (i <= 2)
                    j += rand.Next(0, 3);
                else
                    j -= rand.Next(0, 3);

                activeBrush.Add(i, j);
            }
        }

        foreach (KeyValuePair<Vector2Int, Color> kvp in updateList)
        {
            float changedPixels = 0;
            int firstIndex = 0, lastIndex = 0;

            foreach (KeyValuePair<int, int> coords in activeBrush)
            {
                if (firstIndex == 0)
                    firstIndex = coords.Key;

                lastIndex = coords.Key;
            }

            for (int x = firstIndex * sizeModifier; x < lastIndex * sizeModifier; x++)
                for (int y = -activeBrush[x / sizeModifier] * sizeModifier; y <= activeBrush[x / sizeModifier] * sizeModifier; y++)
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
                Shop.Money += GameManagerScript.GetPrice(level);

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
