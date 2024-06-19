using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerPainting : MonoBehaviour
{
    public List<string> ColorNames;
    public List<Color> Colors;
    public int ActiveColor = 0;
    public int upcomingColor = 0;
    public Color currentColor;
    private Color lastUsedColor;
    public static string[] brushes = new string[4]
    {
        "default", "eclipse", "square", "triangle"
    };
    public static int brushIndex = 0;

    [SerializeField] private List<ColorWheelGun> wheels;
    [SerializeField] Image fillBar;
    [SerializeField] private float ammo;
    [SerializeField] private List<GameObject> paintPrefabs;
    [SerializeField] private GameObject nadePrefab;
    [SerializeField] private Transform mainCam;
    [SerializeField] private float minimumDistance;
    [SerializeField] private GameObject reloadPrompt;

    public float ammoBar;

    [Header("Weapons")]
    [SerializeField] private GameObject standard;
    [SerializeField] private GameObject mini;
    [SerializeField] private GameObject nade;

    [Header("Animators")]
    [SerializeField] private Animator standardAnimator;
    [SerializeField] private Animator nadeAnimator;

    [Header("Spawnpoints")]
    [SerializeField] private Transform standardSpawn;
    [SerializeField] private Transform miniSpawn;
    [SerializeField] private Transform nadeSpawn;

    [Header("Attributes")]
    [SerializeField] private float standardMaxAmmo = 9;
    [SerializeField] private float miniMaxAmmo = 100;

    private readonly float maxTimer = .05f;
    private readonly float maxNadeCD = 2.6f;
    private float timer = .0f;
    private float nadeCD = 2.6f;
    private bool standardActive = true;
    private bool nadeActive = false;

    private Transform spawnpoint;

    private readonly float[] accuracy = new float[3]
    {
        1f, .25f, 2.5f
    };
    private int accuracyIndex = 0;
    private float maxAmmo = 9;
    private float preReloadAmmo;
    private bool reloading;
    private bool reloadFirstFrame;

    private bool barEmptyStart;
    private bool barEmpty;
    private bool startedEmpty;
    private bool mustReload;

    private void Start()
    {
        ammo = maxAmmo;
        spawnpoint = standardSpawn;
    }

    private void Update()
    {
        currentColor = Colors[ActiveColor];

        // Shooting
        if (timer < maxTimer && standardActive)
            timer += Time.deltaTime;
        else if (Input.GetMouseButton(0) && ammo > 0 && !reloading && !mustReload && !nadeActive)
        {
            // Check if we're too close to an object
            if (!Physics.Raycast(mainCam.position, mainCam.forward, minimumDistance))
            {
                Shoot();
                timer = .0f;
            }
        }
        else if (Input.GetMouseButton(0) && nadeActive && nadeCD >= maxNadeCD)
            StartCoroutine(nameof(ThrowNade));

        // Nade cd
        if (nadeCD < maxNadeCD)
            nadeCD += Time.deltaTime;

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && !reloading)
            StartCoroutine(nameof(Reload));

        // Color scroll
        if (Input.mouseScrollDelta.y > 0 && !reloading)
            NextColor();
        else if (Input.mouseScrollDelta.y < 0 && !reloading)
            PrevColor();

        // Update float for visuals for ammo bar and reload prompt
        ammoBar = ammo / maxAmmo;

        if (ammo <= 0)
            mustReload = true;

        // Gun changing
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeGun(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeGun(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeGun(2);

        // Spray modifier
        if (Input.GetKeyDown(KeyCode.Q))
            NextSpray();

        // Brush modifier
        if (Input.GetKeyDown(KeyCode.B))
            NextBrush();

        // Reloading visual
        if (reloading)
        {
            if (!barEmpty)
                barEmptyStart = true;
            else if (!barEmptyStart && reloadFirstFrame)
                startedEmpty = true;

            if (barEmptyStart)
                ammo -= preReloadAmmo * Time.deltaTime / 1.1f;

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
                    ammo += maxAmmo * Time.deltaTime / 1.1f;
                else
                    ammo += maxAmmo * Time.deltaTime / 2.2f;
            }

            reloadFirstFrame = false;
        }

        if (mustReload)
            reloadPrompt.SetActive(true);
        else
            reloadPrompt.SetActive(false);
    }

    private void Shoot()
    {
        ammo -= Time.deltaTime;
        System.Random rand = new();
        int prefabIndex = rand.Next(0, paintPrefabs.Count);

        // Create splatter
        GameObject splatterObject = Instantiate(paintPrefabs[prefabIndex], spawnpoint.position, spawnpoint.rotation);
        PaintSplatter splatter = splatterObject.GetComponent<PaintSplatter>();

        // Determine starting velocity and scale (I use System.Random because it's so much better than Unity's Random)
        float randomForceX, randomForceY, randomForceZ;
        float randomScale = (float)rand.Next(15, 40) / 100;

        if (standardActive)
        {
            randomForceX = rand.Next(-10, 10);
            randomForceY = rand.Next(-10, 10);
            randomForceZ = rand.Next(300, 600);
        }
        else
        {
            randomForceX = rand.Next(-50, 50);
            randomForceY = rand.Next(-50, 50);
            randomForceZ = rand.Next(300, 600);
        }

        randomForceX *= accuracy[accuracyIndex];
        randomForceY *= accuracy[accuracyIndex];

        // Set the starting velocity and scale and send the splatter flying
        Vector3 force = new(randomForceX, randomForceY, randomForceZ);
        Vector3 scale = new(randomScale, randomScale, randomScale);

        if (standardActive)
            splatter.Send(currentColor, force, scale);
        else if (!standardActive)
            splatter.Send(currentColor, force * 1.15f, scale);
    }

    private void NextColor()
    {
        if (!mustReload)
            lastUsedColor = currentColor;

        upcomingColor++;

        if (upcomingColor > Colors.Count - 1)
            upcomingColor = 0;

        mustReload = true;

        foreach (ColorWheelGun wheel in wheels)
            wheel.RotateWheel(1);

        if (mustReload && lastUsedColor == Colors[upcomingColor])
            mustReload = false;
    }

    private void PrevColor()
    {
        if (!mustReload)
            lastUsedColor = currentColor;

        upcomingColor--;

        if (upcomingColor < 0)
            upcomingColor = Colors.Count - 1;

        mustReload = true;

        foreach (ColorWheelGun wheel in wheels)
            wheel.RotateWheel(-1);

        if (mustReload && lastUsedColor == Colors[upcomingColor])
            mustReload = false;
    }

    private void ChangeGun(int gun)
    {
        if (gun == 0 && !standardActive)
            ChangeStandard(true);
        else if (gun == 1 && PlayerUpgrades.Minigun)
            ChangeMini(true);
        else if (gun == 2 && !nadeActive && PlayerUpgrades.Grenades > 0)
            ChangeNade(true);
    }

    private void ChangeStandard(bool newStatus)
    {
        standardActive = newStatus;
        standard.SetActive(standardActive);

        if (standardActive)
        {
            ChangeMini(false);
            ChangeNade(false);
            spawnpoint = standardSpawn;
        }
    }

    private void ChangeMini(bool newStatus)
    {
        mini.SetActive(newStatus);

        if (newStatus)
        {
            ChangeStandard(false);
            ChangeNade(false);
            spawnpoint = miniSpawn;
        }
    }

    private void ChangeNade(bool newStatus)
    {
        nadeActive = newStatus;
        nade.SetActive(nadeActive);

        if (nadeActive)
        {
            ChangeStandard(false);
            ChangeMini(false);
            spawnpoint = nadeSpawn;
        }
    }

    private void NextSpray()
    {
        if (accuracyIndex + 1 > 2)
            accuracyIndex = 0;
        else
            accuracyIndex++;

        if (accuracyIndex == 1 && !PlayerUpgrades.SmallerSpray)
            accuracyIndex++;
        if (accuracyIndex == 2 && !PlayerUpgrades.WiderSpray)
            accuracyIndex = 0;
    }

    private void NextBrush()
    {
        if (brushIndex + 1 > brushes.Length)
            brushIndex = 0;
        else
            brushIndex++;

        if (brushIndex == 1 && !PlayerUpgrades.EclipseBrush)
            brushIndex++;
        if (brushIndex == 2 && !PlayerUpgrades.SquareBrush)
            brushIndex++;
        if (brushIndex == 3 && !PlayerUpgrades.TriangleBrush)
            brushIndex = 0;

    }

    private IEnumerator Reload()
    {
        standardAnimator.SetBool("Reloading", true);
        preReloadAmmo = ammo;
        reloadFirstFrame = true;
        reloading = true;
        barEmpty = ammo <= 0;
        yield return new WaitForSeconds(2.2f);
        ActiveColor = upcomingColor;
        ammo = maxAmmo;
        reloading = false;
        mustReload = false;
        barEmptyStart = false;
        startedEmpty = false;
        standardAnimator.SetBool("Reloading", false);
    }

    private IEnumerator ThrowNade()
    {
        nadeAnimator.SetBool("Throw", true);
        nadeCD = .0f;
        PlayerUpgrades.Grenades--;
        yield return new WaitForSeconds(.7f);
        GameObject thrownNade = Instantiate(nadePrefab, nadeSpawn.position, Quaternion.identity);
        thrownNade.GetComponent<Nade>().SetNade(currentColor, mainCam.rotation);
        yield return new WaitForSeconds(1.9f);
        nadeAnimator.SetBool("Throw", false);

        if (PlayerUpgrades.Grenades == 0)
            ChangeGun(0);
    }
}
