using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPainting : MonoBehaviour
{
    [SerializeField] private List<Color> colors;

    private int activeColor = 0;

    private void Update()
    {
        // Shooting
        if (Input.GetMouseButtonDown(0))
            Shoot();

        // Color scroll
        if (Input.mouseScrollDelta.y > 0)
            NextColor();
        else if (Input.mouseScrollDelta.y < 0)
            PrevColor();
    }

    private void Shoot()
    {

    }

    private void NextColor()
    {
        activeColor++;

        if (activeColor > colors.Count)
            activeColor = 0;
    }

    private void PrevColor()
    {
        activeColor--;

        if (activeColor < 0)
            activeColor = colors.Count - 1;
    }
}
