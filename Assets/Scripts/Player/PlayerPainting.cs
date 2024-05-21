using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPainting : MonoBehaviour
{
    public List<Color> Colors;
    public int ActiveColor = 0;
    public int upcomingColor = 0;
    public Color currentColor;

    [SerializeField] private Transform startPoint;
    [SerializeField] private GameObject gun;
    [SerializeField] private ColorWheelGun wheel;
    [SerializeField] Image fillBar;
    [SerializeField] private float ammo;
    [SerializeField] private GameObject paintPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform mainCam;
    [SerializeField] private float minimumDistance;

    private readonly float maxTimer = .05f;
    private float timer = .0f;

    private float maxAmmo = 9;
    private float preReloadAmmo;
    private bool reloading;
    private bool reloadFirstFrame;
    private bool barEmptyStart;
    private bool barEmpty;
    private bool startedEmpty;
    private bool mustReload;

    public float ammoBar;

    private void Start()
    {
        ammo = maxAmmo;
    }

    private void Update()
    {
        currentColor = Colors[ActiveColor];

        // Shooting
        if (timer < maxTimer)
            timer += Time.deltaTime;
        else if (Input.GetMouseButton(0) && ammo > 0 && !reloading && !mustReload)
        {
            // Check if we're too close to an object
            if (!Physics.Raycast(mainCam.position, mainCam.forward, minimumDistance))
            {
                Shoot();
                timer = .0f;
            }
        }

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
                barEmptyStart = true;
            else if (!barEmptyStart && reloadFirstFrame)
                startedEmpty = true;

            if (barEmptyStart)
                ammo -= preReloadAmmo * Time.deltaTime / 1.5f;

            if (ammo <= 0)
            {
                barEmpty = true;
                barEmptyStart = false;
            }

            if (barEmpty)
            {
                currentColor = Colors[upcomingColor];
                currentColor.a = 1;
                fillBar.color = currentColor;

                if (!startedEmpty)
                    ammo += maxAmmo * Time.deltaTime / 1.5f;
                else
                    ammo += maxAmmo * Time.deltaTime / 3f;
            }

            reloadFirstFrame = false;
        }
    }

    private void Shoot()
    {
        ammo -= Time.deltaTime;

        // Create splatter
        GameObject splatterObject = Instantiate(paintPrefab, spawnPoint.position, spawnPoint.rotation);
        PaintSplatter splatter = splatterObject.GetComponent<PaintSplatter>();

        // Determine starting velocity and scale (I use System.Random because it's so much better than Unity's Random)
        System.Random rand = new();
        int randomForceX = rand.Next(-50, 50);
        int randomForceY = rand.Next(-50, 50);
        int randomForceZ = rand.Next(300, 600);
        float randomScale = (float)rand.Next(15, 40) / 100;

        // Set the starting velocity and scale and send the splatter flying
        Vector3 force = new(randomForceX, randomForceY, randomForceZ);
        Vector3 scale = new(randomScale, randomScale, randomScale);
        splatter.Send(currentColor, force, scale);
    }

    private void NextColor()
    {
        upcomingColor++;

        if (upcomingColor > Colors.Count - 1)
            upcomingColor = 0;

        mustReload = true;
        wheel.RotateWheel(1);
    }

    private void PrevColor()
    {
        upcomingColor--;

        if (upcomingColor < 0)
            upcomingColor = Colors.Count - 1;

        mustReload = true;
        wheel.RotateWheel(-1);
    }

    private IEnumerator Reload()
    {
        preReloadAmmo = ammo;
        reloadFirstFrame = true;
        reloading = true;
        barEmpty = ammo <= 0;
        yield return new WaitForSeconds(3);
        ActiveColor = upcomingColor;
        ammo = maxAmmo;
        reloading = false;
        mustReload = false;
        barEmptyStart = false;
        startedEmpty = false;
    }
}
