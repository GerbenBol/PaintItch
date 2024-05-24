using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureControl : MonoBehaviour
{
    [SerializeField] private AudioClip dingClip;

    public static List<Texture2D> ToUpdate = new();
    public static List<PaintableObject> ToCalculate = new();
    public static AudioClip CompletedDing;

    private static int index = 0;

    private readonly float maxTimer = .1f;
    private float timer = .0f;
    private bool running = false;

    private void Start()
    {
        CompletedDing = dingClip;
    }

    private void Update()
    {
        if (timer > maxTimer)
        {
            if (ToUpdate.Count != 0 && !running)
                StartCoroutine(nameof(ApplyTextures));

            timer = .0f;
        }
        else timer += Time.deltaTime;

        if (index >= ToCalculate.Count)
            StartNextCalc();
    }
    
    private void StartNextCalc()
    {
        PaintableObject obj = ToCalculate[index];
        
        if (obj.CompareTag("Level" + GameManagerScript.CurrentLevel))
        {
            int retVal = obj.CalculatePercentage(1000);

            if (retVal == -1)
                index--;
            else
            {
                index++;
                PaintableObject nextObj = ToCalculate[index];

                if (obj.CompareTag("Level" + GameManagerScript.CurrentLevel))
                {
                    int retValNew = nextObj.CalculatePercentage(retVal);

                    if (retValNew == -1)
                        index--;
                }
            }
        }

        if (index + 1 < ToCalculate.Count)
            index++;
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
