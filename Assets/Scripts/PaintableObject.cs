using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    public Texture2D BaseTexture;
    public Texture2D GreyTexture;
    public Texture2D ColorTexture;

    public void ChangeTexture(Vector2Int coords, Color color)
    {
        ColorTexture.SetPixel(coords.x, coords.y, color);
        TextureControl.ToUpdate.Add(ColorTexture);
    }
}
