using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheelGun : MonoBehaviour
{
    PlayerPainting playerPainting;
    [SerializeField] GameObject wheel;
    void Start()
    {
        playerPainting = GetComponent<PlayerPainting>();
    }

    void Update()
    {
        wheel.transform.Rotate(0f, (365 / 20) * (1 + 1), 0f);
    }
}
