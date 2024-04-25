using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPainting : MonoBehaviour
{
    public List<Color> Colors;
    public int ActiveColor = 0;

    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject gun;
    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private Texture texture;

    private float maxAmmo = 90000;
    [SerializeField] private float ammo;
    private bool reloading;

    public float ammoBar;

    private void Start()
    {
        Colors = new() { Color.red, Color.cyan, Color.blue };
        ammo = maxAmmo;
    }

    private void Update()
    {
        // Shooting
        if (Input.GetMouseButton(0) && ammo > 0 && !reloading)
            Shoot();
        else if (shootParticle.isPlaying)
            shootParticle.Stop();

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && !reloading)
            StartCoroutine(nameof(Reload));

        // Color scroll
        if (Input.mouseScrollDelta.y > 0)
            NextColor();
        else if (Input.mouseScrollDelta.y < 0)
            PrevColor();

        // Update float for visuals for ammo bar
        ammoBar = ammo / maxAmmo;
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
    }

    private void PrevColor()
    {
        ActiveColor--;

        if (ActiveColor < 0)
            ActiveColor = Colors.Count - 1;
    }

    private IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(3);
        ammo = maxAmmo;
        reloading = false;
    }
}
