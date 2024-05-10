using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPainting : MonoBehaviour
{
    public List<Color> Colors;
    public int ActiveColor = 0;
    public Color currentColor;

    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject gun;
    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private Texture texture;
    [SerializeField] private ColorWheelGun wheel;
    [SerializeField] Image fillBar;

    [SerializeField] private float ammo;
    private float maxAmmo = 9;
    private float preReloadAmmo;
    private bool reloading;
    private bool barEmptyStart;
    private bool barEmpty;
    private bool mustReload;
    private int upcomingColor = 0;

    public float ammoBar;

    private void Start()
    {
        ammo = maxAmmo;
        currentColor.a = 1;
    }

    private void Update()
    {
        currentColor = Colors[ActiveColor];
        currentColor.a = 1;

        // Shooting
        if (Input.GetMouseButton(0) && ammo > 0 && !reloading && !mustReload)
            Shoot();
        else if (shootParticle.isPlaying)
            shootParticle.Stop();

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && !reloading)
            StartCoroutine(nameof(Reload));

        // Color scroll
        if (Input.mouseScrollDelta.y > 0 && !reloading)
            NextColor();
        else if (Input.mouseScrollDelta.y < 0 && !reloading)
            PrevColor();

        // Update float for visuals for ammo bar
        ammoBar = ammo / maxAmmo;

        // Reloading visual
        if (reloading)
        {
            if (!barEmpty)
            {
                barEmptyStart = true;
            }
            if (barEmptyStart)
            {
                ammo -= preReloadAmmo * Time.deltaTime / 1.5f;
            }
            if (ammo <= 0)
            {
                barEmpty = true;
            }
            if (barEmpty)
            {
                currentColor = Colors[upcomingColor];
                currentColor.a = 1;
                fillBar.color = currentColor;
                barEmptyStart = false;
                ammo += maxAmmo * Time.deltaTime / 1.5f;
            }
        }
    }

    private void Shoot()
    {
        ammo -= Time.deltaTime;

        if (shootParticle.isStopped)
            shootParticle.Play();
    }

    private void NextColor()
    {
        // ----------------------change to Coroutine or add timer to fix bug where already shot particles can still change color.----------------------------

        upcomingColor++;

        if (upcomingColor > Colors.Count - 1)
            upcomingColor = 0;

        mustReload = true;
        wheel.RotateWheel(1);
    }

    private void PrevColor()
    {
        // ----------------------change to Coroutine or add timer to fix bug where already shot particles can still change color.----------------------------

        upcomingColor--;

        if (upcomingColor < 0)
            upcomingColor = Colors.Count - 1;

        mustReload = true;
        wheel.RotateWheel(-1);
    }

    private IEnumerator Reload()
    {
        preReloadAmmo = ammo;
        reloading = true;
        yield return new WaitForSeconds(3);
        ActiveColor = upcomingColor;
        ammo = maxAmmo;
        reloading = false;
        mustReload = false;
        barEmptyStart = false;
        barEmpty = false;
    }
}
