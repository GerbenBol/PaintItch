using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    public Texture2D BaseTexture;
    public Texture2D GreyTexture;
    public Texture2D ColorTexture;

    private readonly int brushSize = 15;
    private Texture2D texture;
    private readonly Dictionary<Vector2Int, Color> updateList = new();

    private void Start()
    {
        Renderer rend = GetComponent<Renderer>();
        texture = Instantiate(rend.material.mainTexture) as Texture2D;
        rend.material.mainTexture = texture;
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
            for (int x = -brushSize; x <= brushSize; x++)
                for (int y = -brushSize; y <= brushSize; y++)
                    texture.SetPixel(kvp.Key.x + x, kvp.Key.y + y, kvp.Value);

            TextureControl.ToUpdate.Add(texture);
        }
        updateList.Clear();
        yield return null;
    }
}
