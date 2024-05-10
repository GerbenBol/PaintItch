using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheelGun : MonoBehaviour
{
    [SerializeField] GameObject wheel;

    void Update()
    {
        //wheel.transform.Rotate(0f, (365 / 20) * (1), 0f);
    }

    public void RotateWheel(int newColor)
    {
        wheel.transform.Rotate(0, (365 / 20) * newColor, 0f);
    }
}
