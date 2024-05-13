using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] GameObject meter;
    GameObject player;
    private PlayerPainting ammo;

    void Start()
    {
        player = GameObject.Find("Player");
        ammo = player.GetComponent<PlayerPainting>();
    }

    void Update()
    {
        bar.fillAmount = ammo.ammoBar;
        meter.transform.localRotation = Quaternion.Euler(0, 0, 90 - (180 * ammo.ammoBar));
    }
}
