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
    [SerializeField] Image fillBar;

    private float maxAmmo = 9;
    [SerializeField] private float ammo;
    private float preReloadAmmo;
    private bool reloading;
    private bool barEmptyStart;
    private bool barEmpty;
    private bool mustReload;

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
        ActiveColor++;

        if (ActiveColor > Colors.Count - 1)
            ActiveColor = 0;

        mustReload = true;
    }

    private void PrevColor()
    {
        ActiveColor--;

        if (ActiveColor < 0)
            ActiveColor = Colors.Count - 1;

        mustReload = true;
    }

    private IEnumerator Reload()
    {
        preReloadAmmo = ammo;
        reloading = true;
        yield return new WaitForSeconds(3);
        ammo = maxAmmo;
        reloading = false;
        mustReload = false;
        barEmptyStart = false;
        barEmpty = false;
    }
}
