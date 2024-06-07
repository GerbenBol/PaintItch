using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheelGun : MonoBehaviour
{
    public void RotateWheel(int newColor)
    {
        transform.Rotate(0, 0f, (365 / 20) * newColor);
    }
}
