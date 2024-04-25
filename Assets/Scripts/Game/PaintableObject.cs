using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    [SerializeField] private float completionPercentage = .95f;

    public Texture2D MainTexture;

    private bool completed = false;
    private float completedPercentage = .0f;
    private int index;

    private readonly Dictionary<int, int> circle = new()
    {
        { -11, 7 }, { -10, 9 }, { -9, 10 }, { -8, 12 }, { -7, 12 }, { -6, 13 },
        { -5, 14 }, { -4, 14 }, { -3, 15 }, { -2, 15 }, { -1, 15 }, { 0, 15 },
        { 1, 15 }, { 2, 15 }, { 3, 15 }, { 4, 14 }, { 5, 14 }, { 6, 13 },
        { 7, 12 }, { 8, 12 }, { 9, 10 }, { 10, 9 }, { 11, 7 }
    };
    private readonly Dictionary<Vector2Int, Color> updateList = new();

    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        MainTexture = Instantiate(rend.material.mainTexture) as Texture2D;
        rend.material.mainTexture = MainTexture;
        index = GameManagerScript.AddObject();
    }

    public void ChangeTexture(Vector2Int coords, Color color)
    {
        updateList.Add(coords, color);
        StartCoroutine(nameof(UpdateColor));
    }

    private IEnumerator UpdateColor()
    {
        foreach (KeyValuePair<Vector2Int, Color> kvp in updateList)
        {
            float changedPixels = 0;

            foreach (KeyValuePair<int, int> coords in circle)
                for (int y = -coords.Value; y <= coords.Value; y++)
                {
                    MainTexture.SetPixel(kvp.Key.x + coords.Key, kvp.Key.y + y, kvp.Value);
                    changedPixels++;
                }

            float totalPixels = MainTexture.width * MainTexture.height;
            Debug.Log(changedPixels);
            completedPercentage += changedPixels / totalPixels;

            if (completedPercentage >= completionPercentage && !completed)
            {
                completed = true;
                GameManagerScript.CompleteObject(index);
                AudioSource.PlayClipAtPoint(TextureControl.CompletedDing, transform.position);
            }

            if (!TextureControl.ToUpdate.Contains(MainTexture))
                TextureControl.ToUpdate.Add(MainTexture);
        }
        updateList.Clear();
        yield return null;
    }
}
