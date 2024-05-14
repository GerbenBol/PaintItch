using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] GameObject meter;

    private GameObject player;
    private PlayerPainting painting;

    private void Start()
    {
        player = GameObject.Find("Player");
        painting = player.GetComponent<PlayerPainting>();
    }

    private void Update()
    {
        bar.fillAmount = painting.ammoBar;
        meter.transform.localRotation = Quaternion.Euler(0, 0, 90 - (180 * painting.ammoBar));
    }
}
