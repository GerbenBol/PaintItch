using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintableObject : MonoBehaviour
{
    public Texture2D BaseTexture;
    public Texture2D GreyTexture;
    public Texture2D ColorTexture;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("set pixels");
            Renderer rend = GetComponent<Renderer>();

            Texture2D texture = Instantiate(rend.material.GetTexture("_BaseColorMap")) as Texture2D;
            rend.material.SetTexture("_BaseColorMap", texture);

            Color[] cols = texture.GetPixels();
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i] = Color.cyan;
            }
            texture.SetPixels(cols);

            texture.Apply();
        }
    }

    public void ChangeTexture(Vector2Int coords, Color color)
    {
        Debug.Log(ColorTexture.GetPixel(coords.x, coords.y));
        ColorTexture.SetPixel(coords.x, coords.y, color);
        ColorTexture.Apply();
        Debug.Log(ColorTexture.GetPixel(coords.x, coords.y));
    }
}
