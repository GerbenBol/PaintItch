using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] Image _Bar;
    GameObject _Player;
    private PlayerPainting ammo;
    void Start()
    {
        _Player = GameObject.Find("Player");
        ammo = _Player.GetComponent<PlayerPainting>();
    }

    void Update()
    {
        _Bar.fillAmount = ammo.ammoBar;
    }
}
