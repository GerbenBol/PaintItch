using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheelGun : MonoBehaviour
{
    public void RotateWheel(int newColor)
    {
        transform.Rotate(0, (365 / 20) * newColor, 0f);
    }
}
