using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureControl : MonoBehaviour
{
    public static List<Texture2D> ToUpdate = new();

    private readonly float maxTimer = .3f;
    private float timer = .0f;
    private bool running = false;

    private void Update()
    {
        if (timer > maxTimer)
        {
            if (ToUpdate.Count != 0 && !running)
                StartCoroutine(nameof(ApplyTextures));

            timer = .0f;
        }
        else timer += Time.deltaTime;
    }

    private IEnumerator ApplyTextures()
    {
        running = true;

        while (true)
        {
            ToUpdate[0].Apply();
            ToUpdate.Remove(ToUpdate[0]);

            if (ToUpdate.Count != 0)
                yield return new WaitForSeconds(.1f);
            else
                break;
        }
     
        running = false;
    }
}
